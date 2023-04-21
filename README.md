# GTA5 TikTok Integration
GTA5 Plugin to control GTA5 via Webhook events from TikTok LIVE. I recommend [TikFinity](https://tikfinity.zerody.one/) to setup Webhooks on certain events.

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
The basic syntax is: `http://127.0.0.1:6721/<command>:<param>`<br>
All commands are defined in [GTAVWebhookScript.cs](https://github.com/smeysu/GTA5-TikTok-Integration/blob/main/GTAVWebhookScript.cs).<br>

### http://127.0.0.1:6721/kill
Kills the player.

### http://127.0.0.1:6721/spawn_vehicle:Felon
Spawn the specified vehicle near the player. In this case an `Felon` is created.<br>You can find here a list with all vehicle names: https://wiki.gtanet.work/index.php?title=Vehicle_Models

### http://127.0.0.1:6721/remove_spawned_vehicles
Removes all previously spawned vehicles.

### http://127.0.0.1:6721/repair_current_vehicle
Sets the health status of the currently used vehicle to 100%.

### http://127.0.0.1:6721/give_weapon:CombatMG
Gives the player the specified weapon. In this case a `CombatMG`.<br>
You can find a list with all weapon names here: https://wiki.gtanet.work/index.php?title=Weapons_Models

### http://127.0.0.1:6721/set_max_weapon_ammo
Sets the ammunition of the current weapon to the maximum.

### http://127.0.0.1:6721/set_time:02
Sets the time to the specified hour value. In this case to `02` (=02:00) which causes the world to become dark. Use `13` to make it bright again.

### http://127.0.0.1:6721/set_weather:Raining
Sets the weather to the specified value. In this case `Raining`. Available values: `Halloween`, `Clear`, `Unknown`, `Neutral`, `Clearing`, `Blizzard`, `Christmas`, `Clouds`, `ExtraSunny`, `Foggy`, `Overcast`, `Raining`, `Smog`, `Snowing`, `Snowlight`, `ThunderStorm`

### http://127.0.0.1:6721/increase_wanted
Increases the wanted level by 1. There are 5 wanted levels (1-5).

### http://127.0.0.1:6721/decrease_wanted
Decreases the wanted level by 1. There are 5 wanted levels (1-5).

### http://127.0.0.1:6721/max_wanted
Sets the wanted level to 5.

### http://127.0.0.1:6721/add_money:10
Adds the specified amount to the player wallet. In this case, 10 are added. If you want to deduct money, use a negative number value in the URL (e.g. -10).

### http://127.0.0.1:6721/set_money:500
Sets the money balance to the specified value. In this case 500.

### http://127.0.0.1:6721/spawn_attackers:1
Spawn the specified number of attackers to attack you. In this case 1 attacker will be spawned.

### http://127.0.0.1:6721/spawn_attackers_and_shoot:1
Spawn the specified number of attackers to attack you **with guns**. In this case 1 attacker will be spawned.

### http://127.0.0.1:6721/attackers_start_shooting:30
This will give all already spawned attackers a weapon to attack you for a certain amount of time. In this case, for 30 seconds. It can also cause the attackers to shoot each other, which gives you an advantage :)

### http://127.0.0.1:6721/remove_attackers:5
This removes a specified number of previously spawned attackers. In this case 5.

### http://127.0.0.1:6721/leave_car
If you are sitting in a car, this command will kick you out of the car.

### http://127.0.0.1:6721/skydive
Starts a Skydive task.

### http://127.0.0.1:6721/increase_health:20
This will increase or decrease your health with the specified value. In this case, your health will be increased by 20. To reduce the health, use a negative value (e.g. -20).

*More commands are comming soon...*

## TikFinity Integration
The easiest way to trigger the webhooks through TikTok LIVE events is to use [TikFinity](https://tikfinity.zerody.one/). You can integrate up to 5 different webhooks for free. To integrate a webhook do the following:

<img align="right" src="https://user-images.githubusercontent.com/95110801/233436408-4214b3bf-46d6-4ed8-a2d7-9833d11f4b05.png" height=500>

### Create Action
- Go to **Actions & Events**
- Click on **Create new Action**
- Select **Trigger Webhook**
- Paste one of the Webhook URLs above (based on what you want to trigger in the game) and remember to adjust the number in the command if necessary. E.g. how many attackers you want to spawn.
- Scroll down and save the action.

### Create Event
- Click on **Create new Event**
- Define by what the action should be triggered. For example, "Sending a specific gift".
- At "Trigger all of these actions" select the previously created action.
- Scroll down and save the event.

Repeat the steps for all webhooks you want to integrate.

## Support
If you have any questions, you can contact me on Discord: **Smeys#5697**<br>

## Contribute
This is an open source project. If you want to implement new features, feel free to create a pull request.
