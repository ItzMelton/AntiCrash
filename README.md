# AntiCrash
A Tshock terraria plugin that allows to block and stop crashcodes from crashing the mobile users and server. Especially players with PC clients can easily crash mobile users using, projectiles etc in a non-protected TShock server and this is why AntiCrash exist.

## How to Install
1. Download the `.dll` file.
2. Put the `.dll` file inside of `/ServerPlugins/`
3. Stop and rerun the server.

## Versions
AntiCrash v1.0.8     
AntiCrash v1.1.0            
AntiCrash v1.1.2 (Latest)

# Instructions
## Configs
- `Enabled` bool(true/false), setting for turning the plugin off and on
- `MaxMessageLength` int(numbers only), This is where you set the max length of the plugin, whenever the message has the length or higher than the length of the `MaxMessageLength`, it'll automatically kick you. Exp,
```json
{
    "Settings": {
        "MaxMessageLength": 16
    }
}
```
So for example I said `ILoveMaxTheGreat`, and that will be 16 degits, it will automatically kick me. Yes, you can get kicked if the length is higher than 16 or the length you set for `MaxMessageLength`
- `AllowAntiCT` bool(true/false), This is where you turn off and on the anti ct tag setting such as `\[ct:(1|7),(\d*)\]` and `[ct:7,5456]`, This will decrease the protecting from getting crash. I recommended keep this on.
- Default Settings for `TShock/AntiCrash.json`, You can change it in your own choice (optional)
```json
{
  "Settings": {
    "Enabled": true,
    "MaxMessageLength": 50,
    "AllowAntiCT": true
  }
}
```
As you can see `Enabled` is on so you don't have to make it on. `MaxMessageLength` max length for the message is set to 50. `AllowAntiCT` is on for extra protecting.




