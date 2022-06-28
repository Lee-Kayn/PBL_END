using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBL3
{
    public partial class STD_Form : Form
    {
        public string user, pass;
        StudentClass student = new StudentClass();
        public STD_Form(string user,string pass)
        {
            this.user=user;
            this.pass=pass;
            InitializeComponent();
            customizeDesign();
        }
        private void customizeDesign()
        {
            panel_stdsubmenu.Visible = false;
        }
        private void hideSubmenu()
        {
            if (panel_stdsubmenu.Visible == true)
                panel_stdsubmenu.Visible = false;
        }

        private void showSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideSubmenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }
        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_main.Controls.Add(childForm);
            panel_main.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }
        private void but_exit_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.Show();
        }

        private void button_registration_Click(object sender, EventArgs e)
        {
            openChildForm(new Information_STD(user,pass));
            hideSubmenu();
        }

        private void button_manageStd_Click(object sender, EventArgs e)
        {
            openChildForm(new Score_STD(user,pass));
            hideSubmenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Print_STD(user,pass));
            hideSubmenu();
        }

        private void button_std_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_stdsubmenu);
        }

        private void STD_Load(object sender, EventArgs e)
        {
            //label_user.Text = student.welcome(user, pass);
        }

        private void but_dash_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            panel_main.Controls.Add(panel_cover);
        }
    }
}
