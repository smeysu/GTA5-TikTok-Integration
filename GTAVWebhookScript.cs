using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTAVWebhook;
using GTAVWebhook.Types;

public class GTAVWebhookScript : Script
{
    private HttpServer httpServer = new HttpServer();
    private bool isFirstTick = true;
    private List<Vehicle> spawnedVehicles = new List<Vehicle>();
    private List<Attacker> npcList = new List<Attacker>();

    public GTAVWebhookScript()
    {
        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (isFirstTick)
        {
            isFirstTick = false;

            Logger.Clear();

            try
            {
                httpServer.Start();
                Logger.Log("HttpServer listening on port " + httpServer.Port);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to start HttpServer - " + ex.Message);
            }

        }

        while (npcList.Count > 100)
        {
            try
            {
                npcList[0].Remove();
                npcList.RemoveAt(0);
                Logger.Log("Attacker over limit removed");
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to remove old attacker - " + ex.Message);
            }

        }

        try
        {
            foreach (Attacker attacker in npcList)
            {
                attacker.DrawName();
            }
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to draw attacker names - " + ex.Message);
        }

        CommandInfo command = httpServer.DequeueCommand();

        if (command != null)
        {
            try
            {
                ProcessCommand(command);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to execute command " + command.cmd + ". Error: " + ex.Message);
            }
        }

    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {

    }

    private void ProcessCommand(CommandInfo command)
    {
        switch (command.cmd)
        {
            case "kill":
                {
                    Game.Player.Character.Kill();
                    break;
                }
            case "spawn_vehicle":
                {
                    VehicleHash vehicleHash;
                    if (Enum.TryParse<VehicleHash>(command.custom, out vehicleHash))
                    {
                        spawnedVehicles.Add(World.CreateVehicle(new Model(vehicleHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5));
                        Logger.Log("Vehicle spawned: " + command.custom);
                    }
                    else
                    {
                        Logger.Log("Cannot parse vehicle name: " + command.custom);
                    }
                    break;
                }
            case "remove_spawned_vehicles":
                {
                    try
                    {
                        while (spawnedVehicles.Count > 0)
                        {
                            Logger.Log("Removing vehicle: " + spawnedVehicles[0].DisplayName);
                            spawnedVehicles[0].Delete();
                            spawnedVehicles.RemoveAt(0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Failed to remove spawned vehicles: " + ex.Message);
                    }

                    break;
                }
            case "repair_current_vehicle":
                {
                    if (Game.Player.Character.CurrentVehicle != null)
                    {
                        Game.Player.Character.CurrentVehicle.HealthFloat = Game.Player.Character.CurrentVehicle.MaxHealthFloat;
                        Logger.Log("CurrentVehicle Health restored to " + Game.Player.Character.CurrentVehicle.HealthFloat.ToString());
                    }
                    else
                    {
                        Logger.Log("Cannot repair current vehicle because player not in vehicle");
                    }
                    break;
                }
            case "explode_vehicle":
                {
                    Vehicle[] nearbyVehicles = World.GetNearbyVehicles(Game.Player.Character, 20);
                    foreach (Vehicle vehicle in nearbyVehicles)
                    {
                        vehicle.Explode();
                    }
                    break;
                }
            case "give_weapon":
                {
                    WeaponHash weaponHash;
                    if (Enum.TryParse<WeaponHash>(command.custom, out weaponHash))
                    {
                        Game.Player.Character.Weapons.Give(weaponHash, 9999, true, true);
                        Logger.Log("Weapon given: " + command.custom);
                    }
                    else
                    {
                        Logger.Log("Cannot parse weapon name: " + command.custom);
                    }
                    break;
                }
            case "set_max_weapon_ammo":
                {
                    if (!int.TryParse(command.custom, out int ammo))
                        ammo = 9999;

                    if (ammo > Game.Player.Character.Weapons.Current.MaxAmmo)
                        ammo = Game.Player.Character.Weapons.Current.MaxAmmo;

                    Game.Player.Character.Weapons.Current.Ammo = ammo;
                    break;
                }
            case "set_time":
                {
                    TimeSpan ts;
                    if (TimeSpan.TryParse(command.custom + ":00", out ts))
                    {
                        World.CurrentTimeOfDay = ts;
                        Logger.Log("Time set to: " + command.custom);
                    }
                    else
                    {
                        Logger.Log("Cannot parse TimeSpan: " + command.custom);
                    }

                    break;
                }
            case "set_weather":
                {
                    Weather weather;
                    if (Enum.TryParse<Weather>(command.custom, out weather))
                    {
                        World.Weather = weather;
                    }
                    else
                    {
                        Logger.Log("Cannot parse Weather: " + command.custom);
                    }
                    break;
                }
            case "increase_wanted":
                {
                    if (Game.Player.WantedLevel < 5)
                        Game.Player.WantedLevel += 1;
                    break;
                }
            case "decrease_wanted":
                {
                    if (Game.Player.WantedLevel > 0)
                        Game.Player.WantedLevel -= 1;
                    break;
                }
            case "max_wanted":
                {
                    Game.Player.WantedLevel = 5;
                    break;
                }
            case "add_money":
                {

                    if (!int.TryParse(command.custom, out int moneyToAdd))
                    {
                        Logger.Log("add_money needs a numeric value!");
                        break;
                    }

                    Game.Player.Money += moneyToAdd;

                    if (Game.Player.Money < 0)
                        Game.Player.Money = 0;

                    Logger.Log("Player Money set to " + Game.Player.Money.ToString());

                    break;
                }
            case "set_money":
                {

                    if (!int.TryParse(command.custom, out int moneyToSet))
                    {
                        Logger.Log("set_money needs a numeric value!");
                        break;
                    }

                    if (moneyToSet < 0)
                        moneyToSet = 0;

                    Game.Player.Money = moneyToSet;

                    Logger.Log("Player Money set to " + Game.Player.Money.ToString());

                    break;
                }
            case "spawn_attackers":
                {
                    if (Game.Player.Character.IsInAir)
                    {
                        Logger.Log("Cannot spawn attacker because Player IsInAir");
                        break;
                    }

                    if (!int.TryParse(command.custom, out int num))
                        num = 1;

                    if (num > 50) num = 50;

                    for (int i = 0; i < num; i++)
                    {
                        Logger.Log("Spawn Attacker");
                        Attacker npc = new Attacker(command.username, false);
                        npcList.Add(npc);
                    }

                    break;
                }
            case "spawn_attackers_and_shoot":
                {
                    if (Game.Player.Character.IsInAir)
                    {
                        Logger.Log("Cannot spawn attacker because Player IsInAir");
                        break;
                    }

                    if (!int.TryParse(command.custom, out int num))
                        num = 1;

                    if (num > 50) num = 50;

                    for (int i = 0; i < num; i++)
                    {
                        Logger.Log("Spawn Attacker with gun");
                        Attacker npc = new Attacker(command.username, true);
                        npcList.Add(npc);
                    }

                    break;
                }
            case "attackers_start_shooting":
                {
                    if (!int.TryParse(command.custom, out int duration))
                        duration = 30;

                    if (duration < 1) duration = 30;

                    foreach (Attacker attacker in npcList)
                    {
                        Logger.Log("Attacker start shooting");
                        attacker.StartShooting(duration);
                    }

                    break;
                }
            case "remove_attackers":
                {
                    if (!int.TryParse(command.custom, out int num))
                        num = 1;

                    for (int i = 0; i < num; i++)
                    {
                        if (npcList.Count > 0)
                        {
                            npcList[npcList.Count - 1].Remove();
                            npcList.RemoveAt(npcList.Count - 1);

                            Logger.Log("Attacker removed, remaining: " + npcList.Count.ToString());
                        }
                    }

                    break;
                }
            case "leave_car":
                {
                    if (Game.Player.Character.IsInVehicle())
                    {
                        Game.Player.Character.Task.LeaveVehicle(LeaveVehicleFlags.None);
                    }
                    break;
                }
            case "skydive":
                {
                    Game.Player.Character.Position = new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z + 400);
                    Game.Player.Character.Task.Skydive();
                    Logger.Log("Skydive started");
                    break;
                }
            case "increase_health":
                {
                    if (!int.TryParse(command.custom, out int healStep))
                        healStep = 20;

                    float newHealthScore = Game.Player.Character.HealthFloat + healStep;

                    if (newHealthScore < 0)
                        newHealthScore = 0;

                    if (newHealthScore > Game.Player.Character.MaxHealthFloat)
                        newHealthScore = Game.Player.Character.MaxHealthFloat;

                    Game.Player.Character.HealthFloat = newHealthScore;

                    Logger.Log("Health set to " + Game.Player.Character.HealthFloat.ToString());

                    break;
                }
            default:
                {
                    Logger.Log("Unknown Command " + command.cmd);
                    break;
                }
        }
    }
}
