# GTA5 TikTok Integration
GTA5 Plugin to control GTA5 via Webhook events from TikTok Live. You need TikFinity (https://tikfinity.zerody.one/) to setup Webhooks on certain events.

## Installation
- Make sure that GTA5 is not running.
- Download the latest plugin version from the [releases](https://github.com/smeysu/GTA5-TikTok-Integration/releases).
- Extract the downloaded ZIP archive.
- Copy all files and folders into your GTA 5 installation directory (The directory where the **GTA5.exe** is located).<br>
  The default installation directory is: `C:\Program Files\Rockstar Games\Grand Theft Auto V`<br>
  If you get it through Steam, the path is: `E:\SteamLibrary\steamapps\common\Grand Theft Auto V` (The drive letter can vary)
- Restart GTA5 and start playing in offline/story mode.
- You can check if the plugin is running by opening http://127.0.0.1:6721/ in your browser.<br>If everything is working you will get the message `Its working!`.

Please note that the plugin installation may need to be repeated after a GTA5 update, since the GTA5 update process overwrites all files.

## Webhook Usage
The plugin offers several webhook URLs that can be used to trigger different actions in the game.<br>
The basic syntax is: `http://127.0.0.1:6721/<command>:<amount>`<br>
All commands are defined in [GTAVWebhookScript.cs](https://github.com/smeysu/GTA5-TikTok-Integration/blob/main/GTAVWebhookScript.cs).<br>

http://127.0.0.1:6721/kill<br>
Kills the player.

http://127.0.0.1:6721/increase_wanted<br>
Increases the wanted level by 1. There are 5 wanted levels (1-5).

http://127.0.0.1:6721/decrease_wanted<br>
Decreases the wanted level by 1. There are 5 wanted levels (1-5).

http://127.0.0.1:6721/max_wanted<br>
Sets the wanted level to 5.

http://127.0.0.1:6721/add_money:10<br>
Adds the specified amount to the player wallet. In this case, 10 are added. If you want to deduct money, use a negative number value in the URL (e.g. -10).

http://127.0.0.1:6721/set_money:500<br>
Sets the money balance to the specified value. In this case 500.

http://127.0.0.1:6721/spawn_attackers:1<br>
Spawn the specified number of attackers to attack you. In this case 1 attacker will be spawned.

http://127.0.0.1:6721/spawn_attackers_and_shoot:1<br>
Spawn the specified number of attackers to attack you **with guns**. In this case 1 attacker will be spawned.

http://127.0.0.1:6721/attackers_start_shooting:30<br>
This will give all already spawned attackers a weapon to attack you for a certain amount of time. In this case, for 30 seconds. It can also cause the attackers to shoot each other, which gives you an advantage :)

http://127.0.0.1:6721/remove_attackers:5<br>
This removes a specified number of previously spawned attackers. In this case 5.

http://127.0.0.1:6721/leave_car<br>
If you are sitting in a car, this command will kick you out of the car.

http://127.0.0.1:6721/increase_health:20<br>
This will increase or decrease your health with the specified value. In this case, your health will be increased by 20. To reduce the health, use a negative value (e.g. -20).

*More commands are comming soon...*
