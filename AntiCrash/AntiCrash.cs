using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
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
        if (message.Split(" ").Any(substring => substring.Length >= 50)) { // if sent message contains a substring with length greater than 50
            TShock.Players[args.Who].Kick("Sent a message with excessive substring length", true);
            triggered = true;
        }
        if (triggered) {
            args.Handled = true;
        }
    }    
}
