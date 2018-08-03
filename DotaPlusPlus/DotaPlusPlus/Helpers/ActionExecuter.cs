using DotaPlusPlus.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;

namespace DotaPlusPlus.Helpers
{
    class ActionExecuter
    {
        public static bool ExecuteItem(string item, string args, bool qc, Dota2Core core, bool forceCancel)
        {
            Dota2GSI.Nodes.Items items = core.Items();
            VirtualKeyCode key = VirtualKeyCode.VK_S;

            if (items == null) return false;

            int index = -1;

            if (!items.InventoryContains(item)) return false;

            index = items.InventoryIndexOf(item);

            if (index == -1) return false;

            if ((index > 5 || !items.GetInventoryAt(index).CanCast) && (core.mainForm.Cancel() || forceCancel)) return false;

            if (index == 0) key = VirtualKeyCode.VK_Z;
            if (index == 1) key = VirtualKeyCode.VK_X;
            if (index == 2) key = VirtualKeyCode.VK_C;
            if (index == 3) key = VirtualKeyCode.VK_V;
            if (index == 4) key = VirtualKeyCode.VK_B;
            if (index == 5) key = VirtualKeyCode.VK_N;

            if (core.mainForm.SelectHero() && forceCancel) KBMHelper.SelectHero(VirtualKeyCode.F1);

            if (args == "SELF")
            {
                if (qc)
                {
                    KBMHelper.PressAltKey(key);
                }
                else
                {
                    KBMHelper.PressKey(key);
                    KBMHelper.PressKey(key);
                }
            }
            else
            {
                KBMHelper.PressKey(key);
                if (!qc) KBMHelper.LClick();
            }
            return true;
        }

        public static bool ExecuteAbility(string ability, string args, bool qc, Dota2Core core, bool forceCancel)
        {
            Dota2GSI.Nodes.Abilities abilities = core.Abilities();
            VirtualKeyCode key = VirtualKeyCode.VK_S;

            if (abilities == null) return false;

            if (abilities.Count == 4)
            {
                if(ability == "Q")
                {
                    key = VirtualKeyCode.VK_Q;
                    if (!abilities[0].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "W")
                {
                    key = VirtualKeyCode.VK_W;
                    if (!abilities[1].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "E")
                {
                    key = VirtualKeyCode.VK_E;
                    if (!abilities[2].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "R")
                {
                    key = VirtualKeyCode.VK_R;
                    if (!abilities[3].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
            }

            if (abilities.Count == 5)
            {
                if (ability == "Q")
                {
                    key = VirtualKeyCode.VK_Q;
                    if (!abilities[0].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "W")
                {
                    key = VirtualKeyCode.VK_W;
                    if (!abilities[1].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "E")
                {
                    key = VirtualKeyCode.VK_E;
                    if (!abilities[2].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "D")
                {
                    key = VirtualKeyCode.VK_D;
                    if (!abilities[3].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "R")
                {
                    key = VirtualKeyCode.VK_R;
                    if (!abilities[4].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
            }

            if (abilities.Count == 6)
            {
                if (ability == "Q")
                {
                    key = VirtualKeyCode.VK_Q;
                    if (!abilities[0].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "W")
                {
                    key = VirtualKeyCode.VK_W;
                    if (!abilities[1].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "E")
                {
                    key = VirtualKeyCode.VK_E;
                    if (!abilities[2].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "D")
                {
                    key = VirtualKeyCode.VK_D;
                    if (!abilities[3].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "F")
                {
                    key = VirtualKeyCode.VK_F;
                    if (!abilities[4].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
                if (ability == "R")
                {
                    key = VirtualKeyCode.VK_R;
                    if (!abilities[5].CanCast && (core.mainForm.Cancel() || forceCancel)) return false;
                }
            }

            if (ability == "Z") key = VirtualKeyCode.VK_Z;
            if (ability == "X") key = VirtualKeyCode.VK_X;
            if (ability == "C") key = VirtualKeyCode.VK_C;
            if (ability == "V") key = VirtualKeyCode.VK_V;
            if (ability == "B") key = VirtualKeyCode.VK_B;
            if (ability == "N") key = VirtualKeyCode.VK_N;
            if (ability == "A") key = VirtualKeyCode.VK_A;

            if (core.mainForm.SelectHero() && forceCancel) KBMHelper.SelectHero(VirtualKeyCode.F1);

            if (args == "SELF")
            {
                if (qc)
                {
                    KBMHelper.PressAltKey(key);
                }
                else
                {
                    KBMHelper.PressKey(key);
                    KBMHelper.PressKey(key);
                }
            }
            else
            {
                KBMHelper.PressKey(key);
                if (!qc) KBMHelper.LClick();
            }
            return true;
        }
    }
}
