# AntiCrash
A Tshock terraria plugin blocking crashcodes or clients for crashing mobile players and rarely pc (advanced crashes). 


## Special Thanks
* Thanks to `Sors` for fixing the major issues with version v1.1.2 (Patched version : v1.1.5)

## How to Install
1. Download the `.dll` file.
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
- `MaxMessageLengthWithoutSpaces` (int), Setting the max message length for the plugin, Recommended max message length 50 or higher, lowering the max length will decrease its accuracy. Whenever the message has the length or higher than the length of your desired max length, it will automatically kick you. Exp,
```json
{
    "Settings": {
        "MaxMessageLengthWithoutSpaces": 16
    }
}
```
Quick Example : Some random dude said `ILoveMaxTheGreat` in the chat, and that will be about 16 degits, it will be expected to automatically kick that person out of that server he is in. He can get kicked if the length is higher than 16 or the length you set for `MaxMessageLengthWithoutSpaces`
- `AllowAntiCT` (bool), Enable/Disable anti ct tag detectors containing symboled or some kind of projectiles messages, This will decrease the chances from getting crashed. I recommended keep this on.
- `tshock/AntiCrash.json` Default settings, You can change it to your own desire.
```json
{
  "Settings": {
    "Enabled": true,
    "MaxMessageLengthWithoutSpaces": 50,
    "AllowAntiCT": true
  }
}
```
`Enabled` is on so you don't have to make it on. `MaxMessageLengthWithoutSpaces` max length is set to 50. `AllowAntiCT` is enabled.

### Extra
`AntiCrash/Configuration.cs` File credit goes to https://github.com/brianide/CommonGround
I appreciate and thank for those who uses my plugins!
