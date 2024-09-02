using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace AntiCrash;

[ApiVersion(2, 1)]
public class AntiCrash : TerrariaPlugin {
    public override string Name => "AntiCrash";
    public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    public override string Author => "Melton [ Compiled by Nightklp ]";
    public override string Description => "A TShock plugin that attempts to prevent various crash exploits.";
    
    public AntiCrash(Main game) : base(game) {
    }

    public override void Initialize() {
        ServerApi.Hooks.ServerChat.Register(this, OnChat);
        ServerApi.Hooks.NetGetData.Register(this, OnGetData);
    }
    
    // called everytime server receives a chat message before being sent to clients
    public void OnChat(ServerChatEventArgs args) {
        string message = args.Text;
        bool triggered = false;
        if (message.Split(" ").Any(substring => substring.Length >= 50)) { // if sent message contains a substring with length greater than 50
            TShock.Players[args.Who].Kick("Sent a message with excessive substring length", true);
            triggered = true;
        }
        else if (ContainsBadCT(message)) { // if sent message contains a badly formatted ct tag
            TShock.Players[args.Who].Kick("Badly formatted controls touch tag pattern", true);
            triggered = true;
        }
        if (triggered) {
            args.Handled = true;
        }
    }

    // called everytime server receives any type of packets
    public void OnGetData(GetDataEventArgs args) {
        // get a reader for received packet
        using (BinaryReader reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))) {
            if (args.MsgID == PacketTypes.Tile) { // if received packet is a tile modification packet
				reader.BaseStream.Seek(1, SeekOrigin.Begin);
                short x = reader.ReadInt16(); // at 1
				short y = reader.ReadInt16(); // at 3

                if (Main.tile[x, y].type != 88) { // if modified tile is not occupied by a dresser (of any kind)
                    return; // do nothing
                }

                args.Handled = true; // flag the packet as handled before processing it, thus skipping it
                TSPlayer.All.SendTileSquareCentered(x, y, 32);
                TSPlayer.All.SendWarningMessage("Someone tried to do a dresser crash!");
            }

            else if (args.MsgID == PacketTypes.ProjectileNew) { // if received packet is a projectile sync packet
                short UID = reader.ReadInt16(); // at 0
                reader.BaseStream.Seek(8, SeekOrigin.Current); // +8
                float velocityX = reader.ReadSingle(); // at 10
                float velocityY = reader.ReadSingle(); // at 14
                byte owner = reader.ReadByte(); // at 18
                short type = reader.ReadInt16(); // at 19

                if (type == 611) { // if projectile is solar eruption (611)
                    if (velocityX >= 1000 || velocityX <= -1000 || velocityY >= 1000 || velocityY <= -1000) { // if velocity is too high
                        args.Handled = true; // flag the packet as handled before processing it, thus skipping it
                        NetMessage.SendData(29, -1, -1, NetworkText.Empty, UID, owner); // send kill projectile packet
                        TShock.Players[owner].Kick("Solar velocity crash attempt detected", true);
                    }
                }

                else if (type == 409) { // if projectile is razorblade typhoon (409)
                    if (Main.player[owner].HeldItem.type != 2622) { // if held item is not the original item
                        args.Handled = true; // flag the packet as handled before processing it, thus skipping it
                        NetMessage.SendData(29, -1, -1, NetworkText.Empty, UID, owner); // send kill projectile packet
                        TShock.Players[owner].Kick("Shot razorblade typhoon abnormally", true);
                    }
                }
            }
        }
    }

	private static bool ContainsBadCT(string message) {
        string ctPattern = @"\[ct:(1|7),(\d*)\]";
        
        if (Regex.IsMatch(message, @"\[ct:(1|7),$")) {
            return true;
        }

        MatchCollection matches = new Regex(ctPattern).Matches(message);

        foreach (Match match in matches) {
            string itemIDstring = match.Groups[2].Value;
            int itemID;
            if (int.TryParse(itemIDstring, out itemID)) {
                if (itemID < 0 || itemID >= 5456) {
                    return true;
                }
            }
        }

        return false;
    }

    protected override void Dispose(bool disposing) {
        if (disposing) {
            ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
            ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
        }
        base.Dispose(disposing);
    }
}

