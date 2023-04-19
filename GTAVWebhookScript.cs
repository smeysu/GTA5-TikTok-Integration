using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTAVWebhook;
using GTAVWebhook.Types;

public class GTAVWebhookScript : Script
{
    private HttpServer httpServer = new HttpServer();
    private bool isFirstTick = true;
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
                    {
                        Game.Player.Money = 0;
                    }

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
                    {
                        moneyToSet = 0;
                    }

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
            case "increase_health":
                {
                    if (!int.TryParse(command.custom, out int healStep))
                        healStep = 20;

                    float newHealthScore = Game.Player.Character.HealthFloat + healStep;

                    if (newHealthScore < 0)
                    {
                        newHealthScore = 0;
                    }

                    if (newHealthScore > Game.Player.Character.MaxHealthFloat)
                    {
                        newHealthScore = Game.Player.Character.MaxHealthFloat;
                    }

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