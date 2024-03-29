﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using PBL3.BUS;

namespace PBL3
{
    public partial class ManageStudentForm : Form
    {
        StudentClass student = new StudentClass();
        UserClass user= new UserClass();
        CourseClass course = new CourseClass();
        SubjectClass subject = new SubjectClass();
        ScoreClass ScoreClass = new ScoreClass();
        string pre_user, pre_pass;
        public ManageStudentForm()
        {
            InitializeComponent();
        }
        private void ManageStudentForm_Load(object sender, EventArgs e)
        {
            showTable();
        }
        public void showTable()
        {
            DataGridView_student.DataSource = student.getStudentlist(new MySqlCommand("SELECT student.StdId, `StdFirstName`, `StdLastName`, `Birthdate`, `Gender`, `Phone`, `Address`, `UserID`,`Photo` FROM `student`"));
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[8];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void DataGridView_student_Click(object sender, EventArgs e)
        {
            if (DataGridView_student.DataSource == null)
            {
                DataGridView_student.Enabled = false;
            }
            else
            {
                try
                {
                    textBox_id.Text = DataGridView_student.CurrentRow.Cells[0].Value.ToString();
                    textBox_Fname.Text = DataGridView_student.CurrentRow.Cells[1].Value.ToString();
                    textBox_Lname.Text = DataGridView_student.CurrentRow.Cells[2].Value.ToString();


                    dateTimePicker1.Value = (DateTime)DataGridView_student.CurrentRow.Cells[3].Value;
                    if (DataGridView_student.CurrentRow.Cells[4].Value.ToString() == "Male")
                        radioButton_male.Checked = true;

                    textBox_phone.Text = DataGridView_student.CurrentRow.Cells[5].Value.ToString();
                    textBox_address.Text = DataGridView_student.CurrentRow.Cells[6].Value.ToString();
                    string Sub_ID = DataGridView_student.CurrentRow.Cells[7].Value.ToString();
                    byte[] img = (byte[])DataGridView_student.CurrentRow.Cells[8].Value;
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox_student.Image = Image.FromStream(ms);
                    int ID = Convert.ToInt32(DataGridView_student.CurrentRow.Cells[7].Value.ToString());
                    txt_username.Text = user.getUsername(ID);
                    txt_password.Text = user.getPassword(ID);
                    pre_user = txt_username.Text;
                    pre_pass = txt_password.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Score Click", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            

            
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
            pictureBox_student.Image = null;
            txt_password.Clear();
            txt_username.Clear();
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_student.Image = Image.FromFile(opf.FileName);
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_student.DataSource = student.searchStudent(textBox_search.Text);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == "") ||
                (pictureBox_student.Image == null))
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
            string username = txt_username.Text;
            string pass = txt_password.Text;
            string gender = radioButton_male.Checked ? "Male" : "Female";
            int userID = Convert.ToInt32(user.exeCount("SELECT `UserID` FROM `user` WHERE username='" + pre_user + "' AND password='" + pre_pass + "'"));

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
                    MemoryStream ms = new MemoryStream();
                    pictureBox_student.Image.Save(ms, pictureBox_student.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    if (student.updateStudent(id, fname, lname, bdate, gender, phone, address, img) && user.update_user(userID,username,pass))
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


        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (textBox_id.Text == "")
            {
                MessageBox.Show("Field Error- we need student id", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (DataGridViewRow row in DataGridView_student.SelectedRows)
                {
                    int rowId = Convert.ToInt32(row.Cells[0].Value);
                    int userId= Convert.ToInt32(row.Cells[7].Value);    
                    if (rowId > 0)
                    {
                        List<int> list_scoreID = ScoreClass.getList_ScoreID(new MySqlCommand("SELECT `ScoreId` FROM `sub_stu_sco` WHERE StdId='" +rowId+ "'"));
                        foreach(int i in list_scoreID)
                        {
                            ScoreClass.deleteScore(i);
                        }
                        if(student.del_std_sco(rowId))
                        {
                            if(student.deleteStudent(rowId))
                            {
                                if (student.delUserID(userId))
                                    continue;
                            }    
                        }    
                    }
                }
                showTable();
                MessageBox.Show("Student Removed", "Remove Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button_clear.PerformClick();
            }
        }
    }
}
