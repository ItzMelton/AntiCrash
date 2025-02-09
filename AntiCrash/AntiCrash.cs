using System;
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
    
    public override Version Version => new Version(1, 1, 5);
    
    public override string Author => "Melton";
    
    public override string Description => "A TShock plugin that attempts to prevent various crash exploits.";

    private AntiCrashConfig Config;

    public AntiCrash(Main game) : base(game)
    { }

    public override void Initialize()
    {
        /// Create a new config if there is none
        Config = PluginConfiguration.Load<AntiCrashConfig>();

        ServerApi.Hooks.ServerChat.Register(this, OnChat);
        ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
        TShockAPI.Hooks.GeneralHooks.ReloadEvent += OnReload;

        if (!Config.Enabled)
            return;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
            ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
            TShockAPI.Hooks.GeneralHooks.ReloadEvent -= OnReload;
        }
        
        base.Dispose(disposing);
    }

    /// called everytime server receives a chat message
    public void OnChat(ServerChatEventArgs args)
    {
        if (args.Handled)
            return;

        string message = args.Text;
        bool triggered = false;

        if (TShock.Players[args.Who] == null) return;

        /// Detecting long messages (Recommended max message 50 or higher, lower will effect its accuracy)
        if (message.Split(" ").Any(substring => substring.Length >= Config.MaxMessageLengthWithoutSpaces))
        {
            TShock.Players[args.Who].Kick("Sent a message with excessive substring length", true);
    
            triggered = true;
        }
        
        /// Detecting symboled messages 
        else if (ContainsBadCT(message))
        {
            if (!Config.AllowAntiCT) 
                return;
            TShock.Players[args.Who].Kick("Badly formatted controls touch tag pattern", true);
        
            triggered = true;
        }

        /// Detecting short message crashcodes
        // projectiles crash (5456)
        else if (ShortBadCT(message))
        {
            if (!Config.AllowAntiCT) 
                return;
            TShock.Players[args.Who].Kick("Badly short formatted ct tag pattern", true);
            
            triggered = true;
        }
        
        if (triggered)
        {
            args.Handled = true;
        }
    }
    private static bool ContainsBadCT(string message)
    {
        string ctPattern = @"\[ct:(1|7),(\d*)\]";
        /// CT Pattern

        if (Regex.IsMatch(message, @"\[ct:(1|7),$"))
        {
            return true;
        }

        MatchCollection matches = new Regex(ctPattern).Matches(message);

        foreach (Match match in matches)
        {
            string itemIDstring = match.Groups[2].Value;
            int itemID;
            if (int.TryParse(itemIDstring, out itemID))
            {
                if (itemID < 0 || itemID >= 5456)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private static bool ShortBadCT(string message)
    {
        return message.Contains("5456");
    }

    public void OnJoin(JoinEventArgs args)
    {
        var player = TShock.Players[args.Who];
        if (player.Name.Contains("5456"))
        {
            player.Kick("Your name contains a bad character! Please change it to something else.", true);
        }
    }

    public void OnReload(ReloadEventArgs args)
    {
        Config = PluginConfiguration.Load<AntiCrashConfig>();

        if (!Config.Enabled)
        {
            ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
            ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
        }
        else
        {
            ServerApi.Hooks.ServerChat.Register(this, OnChat);
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
        }
    }
}

/// If you want to contact me, here's my information!
/// Discord: itzmelton (Melton)
/// Github: https://github.com/ItzMelton
/// Twitter: https://twitter.com/MeltonTan
/// TCF (Terraria Community Forum): https://forums.terraria.org/index.php?members/itzmelton.317788/
