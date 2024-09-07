using System.IO;
using TShockAPI;
using TShockAPI.Configuration;
/// Config for AntiCrash

namespace AntiCrash
{
    public class AntiCrashConfig
    {
        public static string FilePath = Path.Combine(TShock.SavePath, "AntiCrash.json");
        /// FilePath of the AntiCrash config file
        /// TShock/AntiCrash.json
        
        public bool Enabled { get; set; } = true;
        public int MaxMessageLength { get; set; } = 50;
        public bool AllowAntiCT { get; set; } = true;
        /// Settings for AntiCrash.json
    }

    public class AntiCrashConfigFile : ConfigFile<AntiCrashConfig>
    { }
}
