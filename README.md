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
[AntiCrash v1.1.6](https://github.com/ItzMelton/AntiCrash/releases/tag/v1.1.6) (Pre-release)

# Instructions
## Configs
`Enabled`: Enables or disables the plugin.

`MaxMessageLengthWithoutSpaces`: Sets the maximum allowed message length (excluding spaces). It is recommended to set this to 50 or higher. Lowering this value reduces the plugin's effectiveness. If a message exceeds the specified limit, the sender will be automatically kicked.

`AllowAntiCT`: Enables or disables detection of crash-related messages, such as those containing special symbols or projectiles.

**Default Configuration File**                         
Location: `TShock/AntiCrash.json`                        
You may customize these settings to your preference.                         
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
* Thanks to `Sors89` for fixing major issues.

## Extra
The `AntiCrash/Configuration.cs` File credited goes to https://github.com/brianide/CommonGround                         
Thank you to everyone who uses and supports my plugins!
