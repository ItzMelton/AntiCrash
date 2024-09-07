# AntiCrash
A Tshock terraria plugin that allows to block and stop crashcodes from crashing the mobile users and server. Especially players with PC clients can easily crash mobile users using, projectiles etc in a non-protected TShock server and this is why AntiCrash exist.

## How to Install
1. Download the `.dll` file.
2. Put the `.dll` file inside of `ServerPlugins`
3. Stop and rerun the server.

## Versions
AntiCrash v1.0.8     
AntiCrash v1.1.0

# Instructions
## Configs
- `Enabled` a bool(true/false) setting for turning the plugin off and on
- `MaxMessageLength` This is where you set the max length of the plugin, whenever the message has the length or higher than the length of the `MaxMessageLength`, it'll automatically kick you. Exp,
```json
{
    "Settings": {
        "MaxMessageLength": 16
    }
}
```
So for example I said `ILoveMaxTheGreat`, and that is 16 degits, it will automatically kick me. Yes, you can get kicked if the length is higher than 16 or the length you set for `MaxMessageLength`
- AllowCTTag




