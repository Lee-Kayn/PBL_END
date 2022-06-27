using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PBL3
{
    public partial class LoginForm : Form
    {
        StudentClass student = new StudentClass();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Red;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Transparent;
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            if (textBox_usrname.Text == "" || textBox_password.Text == "")
            {
                MessageBox.Show("Need login data", "Wrong Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string uname = textBox_usrname.Text;
                string pass = textBox_password.Text;
                int Rows = 0;string Role="";
                DataTable table = student.getList(new MySqlCommand("SELECT * FROM `user` WHERE `username`= '" + uname + "' AND `password`='" + pass + "'"));
                Rows = table.Rows.Count;
                Role = student.getRole(uname, pass);
                if (Rows > 0)
                {
                    if(Role== "Admin")
                    {
                        MainForm main = new MainForm();
                        this.Hide();
                        main.Show();
                    }
                    if(Role=="Teacher")
                    {
                        Teacher_Form teach = new Teacher_Form(uname,pass);
                        this.Hide();
                        teach.Show();
                    }    
                    if(Role=="Student")
                    {
                        STD_Form STD = new STD_Form(uname,pass);
                        this.Hide();
                        STD.Show();
                    }    

                }
                else
                {
                    MessageBox.Show("Your username and password are not exists", "Wrong Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
