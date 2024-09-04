using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace AntiCrash;

[ApiVersion(2, 1)]
public class AntiCrash : TerrariaPlugin {
    public override string Name => "AntiCrash";
    public override Version Version => new Version(1, 0, 8);
    public override string Author => "Melton";
    public override string Description => "A TShock plugin that attempts to prevent various crash exploits.";

    public AntiCrash(Main game) : base(game) {
    }

    public override void Initialize() {
        ServerApi.Hooks.ServerChat.Register(this, OnChat);
    }

    // called everytime server receives a chat message before being sent to clients
    public void OnChat(ServerChatEventArgs args) {
        string message = args.Text;
        bool triggered = false;
        if (args.Handled) return;
        if (message.Split(" ").Any(substring => substring.Length >= 50)) {
            // if sent message contains a substring with length greater than 50
            TShock.Players[args.Who].Kick("Sent a message with excessive substring length", true);
            triggered = true;
        }
        else if (ContainsBadCT(message)) {
            // if sent message contains a badly formatted ct tag
            TShock.Players[args.Who].Kick("Badly formatted controls touch tag pattern", true);
            triggered = true;
        }
        if (triggered) {
            args.Handled = true;
        }
    }
    private static bool ContainsBadCT(string message) {
        string ctPattern = @"\[ct:(1|7),(\d*)\]";
        // ct pattern

        if (Regex.IsMatch(message, @"\[ct:(1|7),$")) {
            return true;
        }

        MatchCollection matches = new Regex(ctPattern).Matches(message);

        foreach (Match match in matches) {
            // if the message they sent matches the ct pattern
            string itemIDstring = match.Groups[2].Value;
            int itemID;
            if (int.TryParse(itemIDstring, out itemID)) {
                // if the item ID is a number
                if (itemID < 0 || itemID >= 5456) {
                    return true;
                }
            }
        }
        return false;
    }
}
