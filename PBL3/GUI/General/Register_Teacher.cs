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
    public partial class Register_Teacher : Form
    {
        TeacherClass teacher = new TeacherClass();
        CourseClass course = new CourseClass();
        UserClass userClass = new UserClass();
        SubjectClass subjectClass = new SubjectClass();
        public Register_Teacher()
        {
            InitializeComponent();
        }

        private void Teacher_Load(object sender, EventArgs e)
        {
            showTable();
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
        private void cbb_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string name_course = comboBox1.SelectedItem.ToString();
            string ID_course = course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + name_course + "'");
            foreach (string i in course.getListCourse(new MySqlCommand("SELECT subject.subject_Name FROM `subject` LEFT JOIN teacher_subject ON subject.Subject_ID=teacher_subject.Subject_ID WHERE subject.CourseId='"+ID_course+"' AND ((teacher_subject.Subject_ID) Is Null)")))
            {
                comboBox2.Items.Add(i);
            }
        }
        public void showTable()
        {
            DataGridView_student.DataSource = teacher.getTeacherlist(new MySqlCommand("SELECT * FROM `teacher`"));
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridViewImageColumn dataGridViewImageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn = dataGridViewImageColumn;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        bool verify()
        {
            if ((textBox_Fname.Text == "") || (textBox_Lname.Text == "") ||
                (textBox_phone.Text == "") || (textBox_address.Text == "") ||
                (pic_teacher.Image == null))
            {
                return false;
            }
            else
                return true;
        }
        private void button_upload_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pic_teacher.Image = Image.FromFile(opf.FileName);
        }

        private void button_clear_Click_1(object sender, EventArgs e)
        {
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            txtpassword.Clear();
            txtusername.Clear();
            comboBox1.ResetText();
            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            pic_teacher.Image = null;
        }

        private void button_add_Click_1(object sender, EventArgs e)
        {
            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string phone = textBox_phone.Text;
            string username = txtusername.Text;
            string password = txtpassword.Text;
            string address = textBox_address.Text;
            string subject = comboBox1.Text;
            string gender = radioButton_male.Checked ? "Male" : "Female";


            //we need to check student age between 10 and 100

            int born_year = dateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            if ((this_year - born_year) < 18 || (this_year - born_year) > 100)
            {
                MessageBox.Show("The teacher age must be between 18 and 100", "Invalid Birthdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verify())
            {
                try
                {
                    if (!userClass.Add(username, password, "Teacher"))
                        MessageBox.Show("fail", "Add Teacher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string ID = userClass.exeCount("SELECT `UserID` FROM `user` WHERE username='" + username + "' AND password='" + password + "'");
                    int getID = Convert.ToInt32(ID);
                    // to get photo from picture box
                    MemoryStream ms = new MemoryStream();
                    pic_teacher.Image.Save(ms, pic_teacher.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    if (teacher.insertTeacher(fname, lname, bdate, gender, phone, address, img,getID))
                    {
                        showTable();
                        MessageBox.Show("New Teacher Added", "Add Teacher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    int teacherID = Convert.ToInt32(teacher.exeCount("SELECT `TeacherId` FROM `teacher` WHERE UserID='" + getID + "'"));
                    int course_ID = Convert.ToInt32(course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + comboBox1.SelectedItem.ToString() + "'"));
                    int sub_ID = Convert.ToInt32(subjectClass.exeCount("SELECT `Subject_ID` FROM `subject` WHERE CourseId='" + course_ID.ToString() + "' AND subject_Name='" + comboBox2.SelectedItem.ToString() + "'"));
                    teacher.insertTeacher_Subject(teacherID, sub_ID);
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Field", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
