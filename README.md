# AntiCrash
A TShock plugin that attempts to prevent various crash exploits

## Installation
1. Download the `Anticrash.dll` file.
2. Put the `.dll` file inside of `/ServerPlugins/`
3. Stop and rerun the server.

## Versions
[AntiCrash v1.0.8](https://github.com/ItzMelton/AntiCrash/releases/tag/v1.0.8)    
[AntiCrash v1.1.0](https://github.com/ItzMelton/AntiCrash/releases/tag/v1.1.0)   
[AntiCrash v1.1.2](https://github.com/ItzMelton/AntiCrash/releases/tag/v1.1.2)           
[AntiCrash v1.1.5](https://github.com/ItzMelton/AntiCrash/releases/tag/v1.1.5) (Latest)

# Instructions
## Configs
- `Enabled` (bool), Enable/Disable the plugin
- `MaxMessageLengthWithoutSpaces` (int), Setting the max message length for the plugin, Recommended max message length 50 or higher, lowering the max length will decrease its effect. Whenever the message has the length or higher than the length of your desired max length, it will automatically kick you. Exp,

- `AllowAntiCT` (bool), Enable/Disable anti ct tag detectors containing symboled or projectiles messages.
- `tshock/AntiCrash.json` Default settings, You can change it to whatever you want.
```json
{
  "Settings": {
    "Enabled": true,
    "MaxMessageLengthWithoutSpaces": 50,
    "AllowAntiCT": true
  }
}
```

# Special Thanks
* Thanks to `Sors89` for fixing the major issues with version v1.1.2 (Patched version : v1.1.5)

## Extra
`AntiCrash/Configuration.cs` File credit goes to https://github.com/brianide/CommonGround                         
I appreciate and thank for those who uses my plugins!
