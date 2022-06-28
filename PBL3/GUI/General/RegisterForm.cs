using System;
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
    public partial class RegisterForm : Form
    {
        StudentClass student = new StudentClass();
        UserClass add_User = new UserClass();
        CourseClass course = new CourseClass();
        SubjectClass subject = new SubjectClass();
        int Flag = 0;
        public RegisterForm(int Flag)
        {
            InitializeComponent();
            this.Flag = Flag;
        }



        //create a function to verify
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


        private void RegisterForm_Load(object sender, EventArgs e)
        {
            showTable();
            //getcbbcourse();
        }
        //public void getcbbcourse()
        //{
        //    cbb_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
        //    cbb_course.DisplayMember = "CourseName";
        //    cbb_course.ValueMember = "CourseName";
        //}
        //public void getcbbcourse()
        //{
        //    foreach (string i in course.getListCourse(new MySqlCommand("SELECT `CourseName` FROM `course`")))
        //    {
        //        cbb_course.Items.Add(i);
        //    }
        //    cbb_course.SelectedIndex = 0;
                
        //}
        //private void cbb_course_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cbb_sub.Items.Clear();
        //    string name_course = cbb_course.SelectedItem.ToString();
        //    string ID_course = course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='"+name_course+"'");
        //    foreach (string i in course.getListCourse(new MySqlCommand("SELECT `Subject_Name` FROM `subject` WHERE CourseId='" + ID_course + "'")))
        //    {
        //        cbb_sub.Items.Add(i);
        //    }
        //    cbb_sub.SelectedIndex = 0;
        //}
        // To show student list in DatagridView
        public void showTable()
        {
            DataGridView_student.DataSource = student.getStudentlist(new MySqlCommand("SELECT * FROM `student`"));
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridViewImageColumn dataGridViewImageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn = dataGridViewImageColumn;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            // browse photo from your computer
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                pictureBox_student.Image = Image.FromFile(opf.FileName);

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            // add new student
            string fname = textBox_Fname.Text;
            string lname = textBox_Lname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string phone = textBox_phone.Text;
            string username = txtusername.Text;
            string password = txtpassword.Text;
            string address = textBox_address.Text;
            string gender = radioButton_male.Checked ? "Male" : "Female";
            if (Flag == 0)
            {
                if (!add_User.Add(username, password, "Student"))
                    MessageBox.Show("Fail", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!add_User.Add(username, password, "Teacher"))
                    MessageBox.Show("fail", "Add Teacher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            string ID = add_User.exeCount("SELECT `UserID` FROM `user` WHERE username='"+username+"' AND password='"+password+"'");
            int getID = Convert.ToInt32(ID);

            //we need to check student age between 10 and 100

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
                    // to get photo from picture box
                    MemoryStream ms = new MemoryStream();
                    pictureBox_student.Image.Save(ms, pictureBox_student.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    if (student.insertStudent(fname, lname, bdate, gender, phone, address, img,getID))
                    {
                        showTable();
                        MessageBox.Show("New Student Added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //int stdID = Convert.ToInt32(student.exeCount("SELECT `StdId` FROM `student` WHERE UserID='" + getID + "'"));
                    //int course_ID = Convert.ToInt32(course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + cbb_course.SelectedItem.ToString() + "'"));
                    //int sub_ID = Convert.ToInt32(subject.exeCount("SELECT `Subject_ID` FROM `subject` WHERE CourseId='" + course_ID.ToString() + "' AND subject_Name='" + cbb_sub.SelectedItem.ToString() + "'"));
                    //subject.insertSub_Stu_Sco(stdID, sub_ID);
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

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Fname.Clear();
            textBox_Lname.Clear();
            textBox_phone.Clear();
            textBox_address.Clear();
            txtpassword.Clear();
            txtusername.Clear();

            radioButton_male.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            pictureBox_student.Image = null;
            //cbb_course.SelectedIndex = 0;
        }
    }
}
