using DotaPlusPlus.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotaPlusPlus
{
    public partial class MainForm : Form
    {
        private Dota2Core dota2core;

        public bool Shift()
        {
            return chk_shift.Checked;
        }

        public bool Cancel()
        {
            return chk_cancel.Checked;
        }

        public bool WaitRandom()
        {
            return chk_randomize.Checked;
        }

        public int WaitRandomTime()
        {
            return (int)num_random.Value;
        }

        public bool QuickCast()
        {
            return chk_qcenabled.Checked;
        }

        public ListBox.ObjectCollection Macro()
        {
            return lb_macro.Items;
        }

        public int ActionWait()
        {
            return (int)num_actwait.Value;
        }

        public bool WarnRoshan()
        {
            return chk_warnroshan.Checked;
        }

        public bool WarnAegis()
        {
            return chk_warnaegis.Checked;
        }

        public bool WardWards()
        {
            return chk_wards.Checked;
        }

        public bool WarnRunes()
        {
            return chk_runes.Checked;
        }

        public bool WarnTome()
        {
            return chk_tome.Checked;
        }

        public bool SelectHero()
        {
            return chk_selecthero.Checked;
        }

        public bool StunBreakUlt()
        {
            return chk_stunbreakult.Checked;
        }

        public bool AmAghs()
        {
            return chk_amaghs.Checked;
        }

        public bool DrowAura()
        {
            return chk_drowaura.Checked;
        }

        public bool Buckler()
        {
            return chk_buckler.Checked;
        }

        public bool Silenced()
        {
            return chk_silenced.Checked;
        }

        public ListBox.ObjectCollection SilencedActions()
        {
            return lb_silenced.Items;
        }

        public bool MutedBreak()
        {
            return chk_mutedbreak.Checked;
        }

        public ListBox.ObjectCollection MutedBreakActions()
        {
            return lb_mutedbreak.Items;
        }

        public bool Disarmed()
        {
            return chk_disarmed.Checked;
        }

        public ListBox.ObjectCollection DisarmedActions()
        {
            return lb_disarmed.Items;
        }

        public bool Linkens()
        {
            return chk_linkens.Checked;
        }

        public ListBox.ObjectCollection LinkensActions()
        {
            return lb_linkens.Items;
        }

        public bool HP()
        {
            return chk_hp.Checked;
        }

        public ListBox.ObjectCollection HPActions()
        {
            return lb_hp.Items;
        }

        public int MinHP()
        {
            return (int)num_minhp.Value;
        }

        public int MinHPPercent()
        {
            return (int)num_minhppercent.Value;
        }

        public bool Mana()
        {
            return chk_mana.Checked;
        }

        public ListBox.ObjectCollection ManaActions()
        {
            return lb_mana.Items;
        }

        public int MinMana()
        {
            return (int)num_minmana.Value;
        }

        public int MinManaPercent()
        {
            return (int)num_minmanapercent.Value;
        }

        public bool Disabled()
        {
            return chk_disabled.Checked;
        }

        public ListBox.ObjectCollection DisabledActions()
        {
            return lb_disabled.Items;
        }

        public MainForm()
        {
            InitializeComponent();
            dota2core = new Dota2Core(this);
        }

        private void btn_Q_Click(object sender, EventArgs e)
        {
            string cmd = "Q";
            if (chk_selfcast.Checked) cmd += " SELF";
            tb_action.Text = cmd;
        }

        private void btn_W_Click(object sender, EventArgs e)
        {
            string cmd = "W";
            if (chk_selfcast.Checked) cmd += " SELF";
            tb_action.Text = cmd;
        }

        private void btn_E_Click(object sender, EventArgs e)
        {
            string cmd = "E";
            if (chk_selfcast.Checked) cmd += " SELF";
            tb_action.Text = cmd;
        }

        private void btn_R_Click(object sender, EventArgs e)
        {
            string cmd = "R";
            if (chk_selfcast.Checked) cmd += " SELF";
            tb_action.Text = cmd;
        }

        private void btn_D_Click(object sender, EventArgs e)
        {
            string cmd = "D";
            if (chk_selfcast.Checked) cmd += " SELF";
            tb_action.Text = cmd;
        }

        private void btn_F_Click(object sender, EventArgs e)
        {
            string cmd = "F";
            if (chk_selfcast.Checked) cmd += " SELF";
            tb_action.Text = cmd;
        }


        private void btn_tab_Click(object sender, EventArgs e)
        {
            tb_action.Text = "TAB";
        }

        private void btn_attack_Click(object sender, EventArgs e)
        {
            tb_action.Text = "ATTACK";
        }

        private void btn_wait_Click(object sender, EventArgs e)
        {
            tb_action.Text = "WAIT "+num_time.Value;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_macro.Items.Add(cmd);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            lb_macro.Items.Clear();
        }

        private void btn_shift_Click(object sender, EventArgs e)
        {
            tb_action.Text = "SHIFT";
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            tb_action.Text = "STOP";
        }

        private void btn_z_Click(object sender, EventArgs e)
        {
            tb_action.Text = "Z";
        }

        private void btn_x_Click(object sender, EventArgs e)
        {
            tb_action.Text = "X";
        }

        private void btn_c_Click(object sender, EventArgs e)
        {
            tb_action.Text = "C";
        }

        private void btn_v_Click(object sender, EventArgs e)
        {
            tb_action.Text = "V";
        }

        private void btn_b_Click(object sender, EventArgs e)
        {
            tb_action.Text = "B";
        }

        private void btn_n_Click(object sender, EventArgs e)
        {
            tb_action.Text = "N";
        }

        private void btn_silencedadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_silenced.Items.Add(cmd);
        }

        private void btn_silencedclear_Click(object sender, EventArgs e)
        {
            lb_silenced.Items.Clear();
        }

        private void btn_mutedbreakadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_mutedbreak.Items.Add(cmd);
        }

        private void btn_mutedbreakclear_Click(object sender, EventArgs e)
        {
            lb_mutedbreak.Items.Clear();
        }

        private void btn_disarmedadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_disarmed.Items.Add(cmd);
        }

        private void btn_disarmedclear_Click(object sender, EventArgs e)
        {
            lb_disarmed.Items.Clear();
        }

        private void lb_linkensadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_linkens.Items.Add(cmd);
        }

        private void lb_linkensclear_Click(object sender, EventArgs e)
        {
            lb_linkens.Items.Clear();
        }

        private void btn_hpadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_hp.Items.Add(cmd);
        }

        private void btn_hpclear_Click(object sender, EventArgs e)
        {
            lb_hp.Items.Clear();
        }

        private void btn_manaadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_mana.Items.Add(cmd);

        }

        private void btn_manaclear_Click(object sender, EventArgs e)
        {
            lb_mana.Items.Clear();
        }

        private void btn_disabledadd_Click(object sender, EventArgs e)
        {
            string cmd = tb_action.Text;
            if (tb_action.Text == null) return;
            lb_disabled.Items.Add(cmd);
        }

        private void btn_disabledclear_Click(object sender, EventArgs e)
        {
            lb_disabled.Items.Clear();
        }
    }
}
