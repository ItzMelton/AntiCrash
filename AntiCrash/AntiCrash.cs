using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using CommonGround.Configuration;

namespace AntiCrash;

[ApiVersion(2, 1)]
public class AntiCrash : TerrariaPlugin
{
    public override string Name => "AntiCrash";

    public override Version Version => new Version(1, 1, 5, 4);

    public override string Author => "Melton";

    public override string Description => "A TShock plugin that attempts to prevent various crash exploits.";

    private AntiCrashConfig Config;

    public AntiCrash(Main game) : base(game) { }

    public override void Initialize()
    {
        // Create a new config if there is none
        Config = PluginConfiguration.Load<AntiCrashConfig>();

        if (!Config.Enabled)
            return;

        ServerApi.Hooks.ServerChat.Register(this, OnChat);
        ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
        ServerApi.Hooks.NetGetData.Register(this, OnNetGetData);
        TShockAPI.Hooks.GeneralHooks.ReloadEvent += OnReload;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Config.Enabled)
            {
                ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.NetGetData.Deregister(this, OnNetGetData);
                TShockAPI.Hooks.GeneralHooks.ReloadEvent -= OnReload;
            }
        }

        base.Dispose(disposing);
    }

    // Called everytime server receives a chat message
    public void OnChat(ServerChatEventArgs args)
    {
        if (args.Handled)
            return;

        string message = args.Text;
        bool triggered = false;

        if (TShock.Players[args.Who] == null) 
            return;

        // Detecting long messages (Recommended max message 50 or higher)
        if (message.Split(" ").Any(substring => substring.Length >= Config.MaxMessageLengthWithoutSpaces))
        {
            TShock.Players[args.Who].Kick("Sent a message with excessive substring length", true);
            triggered = true;
        }

        // Detecting symboled messages 
        else if (ContainsBadCT(message))
        {
            if (!Config.AllowAntiCT)
                return;

            TShock.Players[args.Who].Kick("Badly formatted controls touch tag pattern", true);
            triggered = true;
        }

        // Detecting short message crashcodes
        else if (ShortBadCT(message))
        {
            if (!Config.AllowAntiCT) 
                return;

            TShock.Players[args.Who].Kick("Badly short formatted ct tag pattern", true);
            triggered = true;
        }

        if (triggered)
            args.Handled = true;
    }

    public void OnNetGetData(GetDataEventArgs args)
    {
        PacketTypes MsgID = args.MsgID;

        if (args.Handled)
            return;
        
        TSPlayer player = TShock.Players[args.Msg.whoAmI];
        
        if(player.State < 10)
            return;
        
        using (BinaryReader br = new(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
        { 
            switch (args.MsgID)
            {
                case PacketTypes.ChestOpen:
                    if (!Config.AllowAntiCT)
                        return;
                        
                    ChestOpen(player, br);
                    args.Handled = true;
                    return;
            }
        }
    }

    private void ChestOpen(TSPlayer player, BinaryReader br)
    {
        int chestID = br.ReadInt16();
        br.ReadInt16();
        br.ReadInt16();
        byte nameLength = br.ReadByte();
        string chestName = string.Empty;
        if (nameLength != 0)
        {
            if (nameLength <= 20)
                chestName = br.ReadString();
            else if (nameLength != 255)
                nameLength = 0;
        }

        if (chestID == -1)
        {
            int id = player.TPlayer.chest;
            if (id < 0 || id >= Main.chest.Length || Main.chest[id] == null) 
                return;

            Chest chest = Main.chest[id];

            if (ShortBadCT(chestName))
            {
                chest.name = string.Empty;
                TSPlayer.All.SendData(PacketTypes.ChestName, "", id, chest.x, chest.y);

                player.SendErrorMessage("The chest you renamed has been reset to default.");
                TShock.Log.ConsoleWarn($"[AntiCrash] Player {player.Name} renamed a chest containing a crash code at ({chest.x}, {chest.y})");
            }
        }
    }

    private static bool ContainsBadCT(string message)
    {
        string ctPattern = @"\[ct:(1|7),(\d*)\]";
        // CT Pattern

        if (Regex.IsMatch(message, @"\[ct:(1|7),$"))
            return true;

        MatchCollection matches = new Regex(ctPattern).Matches(message);

        foreach (Match match in matches)
        {
            string itemIDstring = match.Groups[2].Value;
            int itemID;
            if (int.TryParse(itemIDstring, out itemID))
            {
                if (itemID < 0 || itemID >= 5456)
                    return true;
            }
        }
        return false;
    }

    private static bool ShortBadCT(string message) => message.Contains("5456");

    public void OnJoin(JoinEventArgs args)
    {
        var player = TShock.Players[args.Who];
        if (ShortBadCT(player.Name))
        {
            player.Kick("Your name contains an unallowed string! Please change it to something else.", true);
        }
    }

    public void OnReload(ReloadEventArgs args)
    {
        Config = PluginConfiguration.Load<AntiCrashConfig>();

        if (!Config.Enabled)
        {
            ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
            ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
            ServerApi.Hooks.NetGetData.Deregister(this, OnNetGetData);
        }
        else
        {
            ServerApi.Hooks.ServerChat.Register(this, OnChat);
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.NetGetData.Register(this, OnNetGetData);
        }
    }
}

/// If you want to contact me, here's my information!
/// Discord: itzmelton (Melton)
/// Github: https://github.com/ItzMelton
/// Twitter: https://twitter.com/MeltonTan
/// TCF (Terraria Community Forum): https://forums.terraria.org/index.php?members/itzmelton.317788/
