using System.ComponentModel;
using System.IO;
using TShockAPI;
using CommonGround.Configuration;
/// Config for AntiCrash

namespace AntiCrash
{
    struct AntiCrashConfig : IPluginConfiguration
    {
        public string FilePath => Path.Combine(TShock.SavePath, "AntiCrash.json");

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [DefaultValue(50)]
        public int MaxMessageLengthWithoutSpaces { get; set; }

        [DefaultValue(true)]
        public bool AllowAntiCT { get; set; }
    }
}
