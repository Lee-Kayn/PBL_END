using MySql.Data.MySqlClient;
using PBL3.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBL3
{
    public partial class ManageTeacher : Form
    {
        TeacherClass teacher = new TeacherClass();
        CourseClass course = new CourseClass();
        UserClass user = new UserClass();
        SubjectClass subjectclass = new SubjectClass();
        public ManageTeacher()
        {
            InitializeComponent();
            getcbbcourse();
        }
        public void getcbbcourse()
        {
            foreach (string i in course.getListCourse(new MySqlCommand("SELECT `CourseName` FROM `course`")))
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
        }
        public void showTable()
        {
            DataGridView_student.DataSource = teacher.getTeacherlist(new MySqlCommand("SELECT `TeacherId`, `Teacher_FN`, `Teacher_LN`, `Birthdate`, `Gender`, `Phone`, `Address`,  `UserID`,`Photo` FROM `teacher`"));
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridViewImageColumn dataGridViewImageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[8];
            imageColumn = dataGridViewImageColumn;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        private void DataGridView_teacher_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_student.CurrentRow.Cells[0].Value.ToString();
            textBox_Fname.Text = DataGridView_student.CurrentRow.Cells[1].Value.ToString();
            textBox_Lname.Text = DataGridView_student.CurrentRow.Cells[2].Value.ToString();

            dateTimePicker1.Value = (DateTime)DataGridView_student.CurrentRow.Cells[3].Value;
            if (DataGridView_student.CurrentRow.Cells[4].Value.ToString() == "Male")
                radioButton_male.Checked = true;

            textBox_phone.Text = DataGridView_student.CurrentRow.Cells[5].Value.ToString();
            textBox_address.Text = DataGridView_student.CurrentRow.Cells[6].Value.ToString();
            comboBox1.ResetText();
            byte[] img = (byte[])DataGridView_student.CurrentRow.Cells[8].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox_student.Image = Image.FromStream(ms);
            int ID = Convert.ToInt32(DataGridView_student.CurrentRow.Cells[7].Value.ToString());
            txtusername.Text = user.getUsername(ID);
            txtpassword.Text = user.getPassword(ID);
        }
        private void manageTeacher_load(object sender, EventArgs e)
        {
            showTable();
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_student.Image = Image.FromFile(opf.FileName);
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            comboBox1.ResetText();
            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            pictureBox_student.Image = null;
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
            string gender = radioButton_male.Checked ? "Male" : "Female";
            string username = txtusername.Text;
            string password = txtpassword.Text;
            string subject = comboBox2.SelectedItem.ToString();
            int userID = Convert.ToInt32(DataGridView_student.CurrentRow.Cells[7].Value.ToString());
            int born_year = dateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            int SubID = Convert.ToInt32(subjectclass.exeCount("SELECT `Subject_ID` FROM `subject` WHERE subject_Name='" + subject + "'"));
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
                    if (teacher.updateTeacher(id, fname, lname, bdate, gender, phone, address, img) && user.update_user(userID, username, password) && subjectclass.Update_SUB(id, SubID)) ;
                    {
                        showTable();
                        MessageBox.Show("Teacher data update", "Update Teacher", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Empty Field", "Update Teacher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (textBox_id.Text == "")
            {
                MessageBox.Show("Field Error- we need Teacher id", "Delete Teacher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (DataGridViewRow row in DataGridView_student.SelectedRows)
                {
                    int rowId = Convert.ToInt32(row.Cells[0].Value);
                    int userId = Convert.ToInt32(row.Cells[7].Value);
                    if (rowId > 0)
                    {
                        if (teacher.del_teacher_subject(rowId))
                        {
                            if (teacher.deleteTeacher(rowId))
                            {
                                if (teacher.delUserID(userId))
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                showTable();
                MessageBox.Show("Teacher Removed", "Remove Teacher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button_clear.PerformClick();
            }    
                
        }
        private void cbb_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string courseName = comboBox1.SelectedItem.ToString();
            string courseID = course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + courseName + "'");
            foreach (string i in course.getListCourse(new MySqlCommand("SELECT `subject_Name`FROM `subject` WHERE CourseId='"+courseID+"'")))
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.SelectedIndex = 0;
        }
    }
}
