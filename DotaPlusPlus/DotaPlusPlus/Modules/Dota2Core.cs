using Dota2GSI;
using Dota2GSI.Nodes;
using Microsoft.Win32;
using DotaPlusPlus.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;

namespace DotaPlusPlus.Modules
{
    class Dota2Core
    {
        public static GameStateListener _gsl;

        public MainForm mainForm;
        private Thread workThread;

        private GameState gs_dump;

        private Dota2GSI.Nodes.Abilities abilities;
        private Dota2GSI.Nodes.Items items;
        private int clockTime = 0;
        private int roshTime = 0;
        private int aegisTime = 0;
        private bool roshToggle = false;
        private int aegisToggle = 3;
        private bool playing = false;
        private bool canSphereTrigger = true;
        private bool alive = false;
        private bool warnWards = false;
        private int wardsCD = 0;
        private bool canTimeTrigger = true;
        private bool comboLock = false;
        private bool autoShadowAmulet = false;
        private ushort port = 4702;

        public Dota2GSI.Nodes.Abilities Abilities()
        {
            return abilities;
        }

        public Dota2GSI.Nodes.Items Items()
        {
            return items;
        }
        public Dota2Core(MainForm reference)
        {
            this.mainForm = reference;
            CreateGsifile();

            Process[] pname = Process.GetProcessesByName("Dota2");
            if (pname.Length == 0)
            {
            }

            _gsl = new GameStateListener(port);
            _gsl.NewGameState += OnNewGameState;


            if (!_gsl.Start())
            {
            }

            workThread = new Thread(Run);
            if (workThread != null)
            {
                workThread.IsBackground = true;
                workThread.Start();
            }


        }

        private void OnNewGameState(GameState gs)
        {

            gs_dump = gs;

            if (gs.IsSpectator)
            {
                playing = false;
            }
            else
            {
                abilities = gs.Abilities;
                items = gs.Items;
                playing = true;
                clockTime = gs.Map.ClockTime;
                alive = gs.Hero.IsAlive;
                wardsCD = gs.Map.Ward_Purchase_Cooldown;
                if (wardsCD > 0)
                {
                    warnWards = true;
                }
                if (clockTime % 2 == 1)
                {
                    canTimeTrigger = true;
                }
                if (alive)
                {
                    //URSA AGHS
                    if (mainForm.StunBreakUlt() && gs.Hero.Name == "npc_dota_hero_ursa" && gs.Hero.IsStunned && items.InventoryContains("item_ultimate_scepter"))
                    {
                        if (items.InventoryIndexOf("item_ultimate_scepter") < 6)
                        {
                            ActionExecuter.ExecuteAbility("R", "", mainForm.QuickCast(), this, true);
                        }
                    }
                    //ABBA ULT
                    if (mainForm.StunBreakUlt() && gs.Hero.Name == "npc_dota_hero_abaddon" && (gs.Hero.IsStunned || gs.Hero.IsBreak))
                    {
                        ActionExecuter.ExecuteAbility("R", "", mainForm.QuickCast(), this, true);
                    }
                    //AEON DISK
                    /*if (main form check field && gs.Items.InventoryContains("item_aeon_disk"))
                    {
                        int index = items.InventoryIndexOf("item_aeon_disk");
                        Item aeonDisk = gs.Items.GetInventoryAt(index);
                        if (aeonDisk.Cooldown > 0 && canAeonDiskTrigger && index < 6)
                        {
                            if (mainForm.SelectHero()) KBMHelper.SelectHero(VirtualKeyCode.F1);
                            ActionExecuter.ExecuteItem("item_black_king_bar", "", mainForm.QuickCast(), this, true);
                            canAeonDiskTrigger = false;
                        }
                        if (aeonDisk.Cooldown == 0)
                        {
                            canAeonDiskTrigger = true;
                        }
                    }*/
                    //DROW AURA
                    if (mainForm.DrowAura() && gs.Hero.Name == "npc_dota_hero_drow_ranger")
                    {
                        ActionExecuter.ExecuteAbility("E", "", mainForm.QuickCast(), this, true);
                    }

                    if (mainForm.Buckler())
                    {
                        ActionExecuter.ExecuteItem("item_buckler", "", mainForm.QuickCast(), this, true);
                    }
                    //LINKENS AND AM AGHS
                    if (((mainForm.AmAghs() && gs.Hero.Name == "npc_dota_hero_antimage" && items.InventoryContains("item_ultimate_scepter")) || items.InventoryContains("item_sphere")) && mainForm.Linkens())
                    {
                        string name;
                        int cd = 0;
                        int index = 6;
                        if (items.InventoryContains("item_sphere"))
                        {
                            name = "item_sphere";
                            index = items.InventoryIndexOf(name);
                            cd = items.GetInventoryAt(index).Cooldown;
                            if (index > 5 && mainForm.AmAghs() && gs.Hero.Name == "npc_dota_hero_antimage")
                            {
                                name = "item_ultimate_scepter";
                                index = items.InventoryIndexOf(name);
                                cd = abilities[2].Cooldown;
                            }
                        }
                        else
                        {
                            name = "item_ultimate_scepter";
                            index = items.InventoryIndexOf(name);
                            cd = abilities[2].Cooldown;
                        }

                        if (canSphereTrigger && index < 6 && cd > 0)
                        {
                            SimpleParser(mainForm.LinkensActions());

                            canSphereTrigger = false;
                        }

                        if (cd == 0)
                        {
                            canSphereTrigger = true;
                        }
                    }

                    if (gs.Hero.IsSilenced && mainForm.Silenced()) SimpleParser(mainForm.SilencedActions());
                    if ((gs.Hero.IsMuted || gs.Hero.IsBreak) && mainForm.MutedBreak()) SimpleParser(mainForm.MutedBreakActions());
                    if (gs.Hero.IsDisarmed && mainForm.Disarmed()) SimpleParser(mainForm.DisarmedActions());
                    if (mainForm.HP() && (gs.Hero.Health <= mainForm.MinHP()|| gs.Hero.HealthPercent <= mainForm.MinHPPercent())) SimpleParser(mainForm.HPActions());
                    if (mainForm.Mana() && (gs.Hero.Mana <= mainForm.MinMana() || gs.Hero.ManaPercent <= mainForm.MinManaPercent())) SimpleParser(mainForm.ManaActions());
                    if ((gs.Hero.IsStunned /*|| gs.Hero.IsHexed*/) && mainForm.Disabled()) SimpleParser(mainForm.DisabledActions());
                }
            }
        }

        public void DumpGS()
        {
            try
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\gs_dump." + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + ".json", gs_dump.ToString());
            } catch
            {

            }
        }


        private void CreateGsifile()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");

            if (regKey != null)
            {
                string gsifolder = regKey.GetValue("SteamPath") +
                                   @"\steamapps\common\dota 2 beta\game\dota\cfg\gamestate_integration";
                Directory.CreateDirectory(gsifolder);
                string gsifile = gsifolder + @"\gamestate_integration_D2C.cfg";
                if (File.Exists(gsifile))
                    return;

                string[] contentofgsifile =
                {
                    "\"Dota 2 Integration Configuration\"",
                    "{",
                    "    \"uri\"           \"http://localhost:"+port+"\"",
                    "    \"timeout\"       \"5.0\"",
                    "    \"buffer\"        \"0.1\"",
                    "    \"throttle\"      \"0.1\"",
                    "    \"heartbeat\"     \"30.0\"",
                    "    \"data\"",
                    "    {",
                    "        \"provider\"      \"1\"",
                    "        \"map\"           \"1\"",
                    "        \"buildings\"     \"1\"",
                    "        \"player\"        \"1\"",
                    "        \"hero\"          \"1\"",
                    "        \"abilities\"     \"1\"",
                    "        \"items\"         \"1\"",
                    "        \"draft\"         \"1\"",
                    "        \"wearables\"     \"1\"",
                    "    }",
                    "}",

                };

                File.WriteAllLines(gsifile, contentofgsifile);
            }
            else
            {
            }
        }

        private void Run()
        {
            while (true)
            {
                if (playing)
                {
                    if (wardsCD == 0 && warnWards && mainForm.WardWards())
                    {
                        warnWards = false;
                        string chatStr = "▶ Wards are off cooldown!";
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(50);
                        KBMHelper.TypeChat(chatStr);
                        Thread.Sleep(50);
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(200);
                    }
                    if (canTimeTrigger)
                    {
                        if (mainForm.WarnRunes() && mainForm.WarnTome() && clockTime % 600 == 0 && clockTime > 599)
                        {
                            string chatStr = "▶ All Runes are up and Tome of Knowledge can be purchased!";
                            canTimeTrigger = false;
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(200);
                        }
                        else if (!mainForm.WarnRunes() && mainForm.WarnTome() && clockTime % 600 == 0 && clockTime > 599)
                        {
                            string chatStr = "▶ Tome of Knowledge can be purchased!";
                            canTimeTrigger = false;
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(200);
                        }
                        else if (mainForm.WarnRunes() && !mainForm.WarnTome() && clockTime % 600 == 0 && clockTime > 599)
                        {
                            string chatStr = "▶ All Runes are up!";
                            canTimeTrigger = false;
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(200);
                        }
                        else if (mainForm.WarnRunes() && clockTime % 120 == 0 && clockTime % 600 != 0 && clockTime > 119)
                        {
                            string chatStr = "▶ Power-Up Runes are up!";
                            canTimeTrigger = false;
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(200);
                        }
                        else if (mainForm.WarnRunes() && clockTime % 300 == 0 && clockTime % 600 != 0 && clockTime > 299)
                        {
                            string chatStr = "▶ Bounty Runes are up!";
                            canTimeTrigger = false;
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(200);
                        }
                    }

                    if (mainForm.WarnRoshan() && clockTime >= roshTime && roshToggle)
                    {
                        int limit = roshTime + 180;
                        string chatStr = "▶ Roshan might be up! Limit is: " + TimeHelper.toHHMMSS(limit);
                        roshToggle = false;
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(50);
                        KBMHelper.TypeChat(chatStr);
                        Thread.Sleep(50);
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(200);
                    }

                    if (mainForm.WarnAegis() && clockTime <= aegisTime && aegisToggle > 0)
                    {
                        string chatStr = "";
                        int left = aegisTime - clockTime;
                        if (left <= 0 && aegisToggle == 1)
                        {
                            chatStr = "▶ Aegis expired!";
                            aegisToggle = 0;
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        }
                        else if (left == 60 && aegisToggle == 2)
                        {
                            chatStr = "▶ Aegis will expire in " + left + " seconds! (" + TimeHelper.toHHMMSS(aegisTime) + ")";
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            aegisToggle = 1;
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        }
                        else if (left == 180 && aegisToggle == 3)
                        {
                            chatStr = "▶ Aegis will expire in " + left + " seconds! (" + TimeHelper.toHHMMSS(aegisTime) + ")";
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                            aegisToggle = 2;
                            Thread.Sleep(50);
                            KBMHelper.TypeChat(chatStr);
                            Thread.Sleep(50);
                            KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        }
                        Thread.Sleep(200);
                    }

                    if (KBMHelper.GetAsyncKeyState(0x2D) < 0) //INSERT
                    {
                        string chatStr = "▶ Roshan Timer: ";
                        int minTime = clockTime + 480; //8 MINS
                        int maxTime = clockTime + 660; //11 MINS
                        roshTime = minTime;
                        roshToggle = true;
                        chatStr = chatStr + TimeHelper.toHHMMSS(minTime) + " - " + TimeHelper.toHHMMSS(maxTime);
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(50);
                        KBMHelper.TypeChat(chatStr);
                        Thread.Sleep(50);
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(200);
                        continue;
                    }

                    if (KBMHelper.GetAsyncKeyState(0x2E) < 0) //DELETE
                    {
                        string chatStr = "▶ Aegis expires on: ";
                        int minTime = clockTime + 300; //5 MINS
                        aegisTime = minTime;
                        aegisToggle = 3;
                        chatStr = chatStr + TimeHelper.toHHMMSS(minTime);
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(50);
                        KBMHelper.TypeChat(chatStr);
                        Thread.Sleep(50);
                        KBMHelper.PressKey(VirtualKeyCode.RETURN);
                        Thread.Sleep(200);
                        continue;
                    }

                    if (KBMHelper.GetAsyncKeyState(0x5) < 0 && alive && !comboLock) //MOUSE4 and ALIVE
                    {
                        KBMHelper.keybd_event(0x5, 0, 0x2, 0);
                        ListBox.ObjectCollection macro;
                        macro = mainForm.Macro();
                        int time = 0;
                        string command = "";
                        string args = "";
                        bool success = false;
                        bool waited = false;
                        bool shiftState = false;
                        comboLock = true;
                        foreach (string macroCMD in macro)
                        {
                            if (macroCMD == null || macroCMD.Length == 0) continue;
                            command = "";
                            args = "";
                            waited = false;
                            if (macroCMD.Split(' ').Length == 1)
                            {
                                command = macroCMD.Split(' ')[0];
                            }
                            else if (macroCMD.Split(' ').Length >= 2)
                            {
                                command = macroCMD.Split(' ')[0];
                                args = macroCMD.Split(' ')[1];
                            }

                            if (command == "ATTACK")
                            {
                                ActionExecuter.ExecuteAbility(command.Substring(0, 1), args, mainForm.QuickCast(), this, false);
                                success = true;
                            }
                            else if (command == "TAB")
                            {
                                KBMHelper.PressKey(VirtualKeyCode.TAB);
                                waited = true;
                                success = true;
                                continue;
                            }
                            else if (command == "STOP")
                            {
                                KBMHelper.PressKey(VirtualKeyCode.VK_S);
                                shiftState = !shiftState;
                                waited = true;
                                success = true;
                                continue;
                            }
                            else if (command == "SHIFT")
                            {
                                if (!shiftState)
                                {
                                    KBMHelper.PressKey(VirtualKeyCode.VK_S);
                                    KBMHelper.ShiftDown();
                                }
                                else
                                {
                                    KBMHelper.ShiftUp();
                                    KBMHelper.PressKey(VirtualKeyCode.VK_S);
                                }
                                shiftState = !shiftState;
                                waited = true;
                                success = true;
                                continue;
                            }
                            else if (command == "WAIT" && success)
                            {
                                time = Int32.Parse(args);
                                waited = true;
                                Thread.Sleep(time);
                                KBMHelper.PressKey(VirtualKeyCode.VK_S);
                                continue;
                            }
                            success = true;

                            if (command.Length == 1)
                            {
                                success = ActionExecuter.ExecuteAbility(command.Substring(0, 1), args, mainForm.QuickCast(), this, false);
                            }

                            if (command.Length > 4)
                            {
                                if (command.Substring(0, 5) == "item_")
                                {
                                    success = ActionExecuter.ExecuteItem(command, args, mainForm.QuickCast(), this, false);
                                }
                            }

                            if (!success && mainForm.Cancel()) break;

                            //if (KBMHelper.GetAsyncKeyState(0x48) < 0) break; //H - HALT

                            if (!waited && success)
                            {
                                Random rnd = new Random();
                                time = mainForm.ActionWait();
                                if (mainForm.WaitRandom()) time += rnd.Next(1, mainForm.WaitRandomTime());
                                if (shiftState && !mainForm.Shift()) time = 0;
                                Thread.Sleep(time);
                            }

                        }

                        if (shiftState) KBMHelper.ShiftUp();
                        comboLock = false;

                        Thread.Sleep(200);

                    }

                    if (KBMHelper.GetAsyncKeyState(0x20) < 0)
                    {
                        KBMHelper.keybd_event(0x20, 0, 0x2, 0);
                        autoShadowAmulet = !autoShadowAmulet;
                        Thread.Sleep(200);
                    }

                    if (autoShadowAmulet && KBMHelper.GetAsyncKeyState(0x2) < 0)
                    {
                        Random rnd = new Random();
                        int time = 200;
                        if (mainForm.WaitRandom()) time += rnd.Next(1, mainForm.WaitRandomTime());
                        ActionExecuter.ExecuteItem("item_shadow_amulet", "SELF", mainForm.QuickCast(), this, false);
                        Thread.Sleep(time);
                    }

                    if (mainForm.Phase() && (KBMHelper.GetAsyncKeyState(0x2) < 0 || KBMHelper.GetAsyncKeyState(0x41) < 0 || KBMHelper.GetAsyncKeyState(0x4D) < 0))
                    {
                        Random rnd = new Random();
                        int time = 200;
                        if (mainForm.WaitRandom()) time += rnd.Next(1, mainForm.WaitRandomTime());
                        ActionExecuter.ExecuteItem("item_phase_boots", null, mainForm.QuickCast(), this, true);
                        Thread.Sleep(time);
                    }

                }
                Thread.Sleep(1);
            }
        }

        private void SimpleParser(ListBox.ObjectCollection actions)
        {
            bool success = false;
            string command = "";
            string args = "";
            if (!canTimeTrigger) return;
            foreach (string action in actions)
            {
                if (action == null || action.Length == 0) continue;
                args = "";
                if (action.Split(' ').Length == 1)
                {
                    command = action.Split(' ')[0];
                }
                else if (action.Split(' ').Length >= 2)
                {
                    command = action.Split(' ')[0];
                    args = action.Split(' ')[1];
                }
                if (command.Length == 1)
                {
                    success = ActionExecuter.ExecuteAbility(command.Substring(0, 1), args, mainForm.QuickCast(), this, true);
                }

                if (command.Length > 4)
                {
                    if (command.Substring(0, 5) == "item_")
                    {
                        success = ActionExecuter.ExecuteItem(command, args, mainForm.QuickCast(), this, true);
                    }
                }
                if (success)
                {
                    canTimeTrigger = false;
                    return;
                }
            }
        }
    }
}
