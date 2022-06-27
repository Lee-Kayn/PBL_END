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
    public partial class Information_STD : Form
    {
        StudentClass student =new StudentClass();
        public string user, pass;
        public Information_STD(string user,string pass)
        {
            this.user = user;
            this.pass = pass;
            InitializeComponent();
        }

        private void Clickk(object sender, EventArgs e)
        {
            textBox_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox_Fname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Lname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            dateTimePicker1.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;
            if (dataGridView1.CurrentRow.Cells[4].Value.ToString() == "Male")
                radioButton_male.Checked = true;

            textBox_phone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtusername.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            txtpass.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBox_address.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
        }

        bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == ""))
            {
                return false;
            }
            else
                return true;
        }
        private void button_update_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox_id.Text);
            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string phone = textBox_phone.Text;
            string address = textBox_address.Text;
            string gender = radioButton_male.Checked ? "Male" : "Female";
            string user=txtusername.Text;
            string pass = txtpass.Text;
            this.user = user;
            this.pass = pass;

            int born_year = dateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            if ((this_year - born_year) < 10 || (this_year - born_year) > 100)
            {
                MessageBox.Show("The student age must be between 10 and 100", "Invalid Birthdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verify())
            {
                try
                {
                    if (student.updateStudent_STD(id, fname, lname, bdate, gender, phone,user,pass ,address))
                    {
                        showTable();
                        MessageBox.Show("Student data update", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button_clear.PerformClick();
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Field", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            showTable();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource= student.getStudentlist(new MySqlCommand("SELECT StdId,StdFirstName,StdLastName,Birthdate,Gender,Phone,username,password,Address FROM `student` where username='"+user+"' and password='"+pass+"'"));
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
        }

        public void showTable()
        {
            dataGridView1.DataSource = student.getStudentlist(new MySqlCommand("SELECT StdId,StdFirstName,StdLastName,Birthdate,Gender,Phone,username,password,Address FROM `student` where username='" + user + "' and password='" + pass + "'"));
        }

    }
}
