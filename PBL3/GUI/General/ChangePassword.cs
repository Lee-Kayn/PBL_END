using MySql.Data.MySqlClient;
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
    public partial class ChangePassword : Form
    {
        DBconnect connect = new DBconnect();

        string user, pass;
        int Flag;
        StudentClass student=new StudentClass();
        TeacherClass teacher=new TeacherClass();
        private void button_print_Click(object sender, EventArgs e)
        {
            string old_pass = textBox1.Text;
            string new_pass = textBox2.Text;
            string confirm_pass = textBox3.Text;
            if(old_pass == pass && new_pass !="")
                if(confirm_pass == new_pass)
                {
                    if(Flag == 0)
                    {
                        MySqlCommand command = new MySqlCommand("UPDATE `student` SET `password` = '" + new_pass + "' WHERE `student`.`StdId` = " + student.STD_ID(user, pass), connect.getconnection);
                        connect.openConnect();
                        int i = command.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand("UPDATE `teacher` SET `password` = '" + new_pass + "' WHERE `teacher`.`TeacherId` = " + teacher.getTeacherid(user, pass), connect.getconnection);
                        connect.openConnect();
                        int i = command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Change Password Successfully", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pass = new_pass;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                } 
                else
                {
                    MessageBox.Show("New Password is not the same Confirm Password ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
            {
                if(old_pass=="")
                    MessageBox.Show("Old Password is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("Old Password is not successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            this.Close();
        }

        public ChangePassword(string user,string pass,int Flag)
        {
            this.user = user;
            this.pass = pass;
            this.Flag = Flag;
            InitializeComponent();
        }
    }
}
