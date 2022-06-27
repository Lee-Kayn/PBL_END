using MySql.Data.MySqlClient;
using PBL3.GUI.Teacher;
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
    public partial class Teacher_Form : Form
    {
        StudentClass student = new StudentClass();
        CourseClass course = new CourseClass();
        TeacherClass teacher = new TeacherClass();
        string user, pass;
        public Teacher_Form(string uname,string password)
        {
            InitializeComponent();
            customizeDesign();
            user = uname;
            pass= password;
        }

        private void Teacher_Form_Load(object sender, EventArgs e)
        {
            //studentCount();
            //label_user.Text = teacher.welcome(user,pass);
            ////populate the combobox with courses name
            //comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `teacher` where username='"+user+"' and password='"+pass+"'"));
            //comboBox_course.DisplayMember = "Subject";
            //comboBox_course.ValueMember = "Subject";
        }

        //create a function to display student count
        private void studentCount()
        {
            //Display the values
            label_totalStd.Text = "Total Students : " + student.totalStudent();
            label_maleStd.Text = "Male : " + student.maleStudent();
            label_femaleStd.Text = "Female : " + student.femaleStudent();

        }


        private void customizeDesign()
        {
            panel_stdsubmenu.Visible = false;
            panel_scoreSubmenu.Visible = false;
        }

        private void hideSubmenu()
        {
            if (panel_stdsubmenu.Visible == true)
                panel_stdsubmenu.Visible = false;
            if (panel_scoreSubmenu.Visible == true)
                panel_scoreSubmenu.Visible = false;
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

        private void button_std_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_stdsubmenu);
        }
        #region StdSubmenu

        private void button_manageStd_Click(object sender, EventArgs e)
        {
            teacher.Flag = 1;
            openChildForm(new ManageStudentForm());
            hideSubmenu();
        }

        private void button_stdPrint_Click(object sender, EventArgs e)
        {
            teacher.Flag = 1;
            openChildForm(new PrintStudent());
            hideSubmenu();
        }

        #endregion StdSubmenu

        private void button_score_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_scoreSubmenu);
        }
        #region ScoreSubmenu
        private void button_newScore_Click(object sender, EventArgs e)
        {
            openChildForm(new ScoreForm(1));
            //...
            //..Your code
            //...
            hideSubmenu();
        }

        private void button_manageScore_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageScoreForm(1));

            hideSubmenu();
        }

        private void button_scorePrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintScoreForm());
            hideSubmenu();
        }


        #endregion ScoreSubmenu

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
        private void comboBox_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            //label_cmale.Text = "Male : " + student.exeCount("SELECT COUNT(*) FROM student INNER JOIN score ON score.StudentId = student.StdId WHERE score.CourseName = '" + comboBox_course.Text + "' AND student.Gender = 'Male'");
            //label_cfemale.Text = "Female : " + student.exeCount("SELECT COUNT(*) FROM student INNER JOIN score ON score.StudentId = student.StdId WHERE score.CourseName = '" + comboBox_course.Text + "' AND student.Gender = 'Female'");
        }

        private void but_exit_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            panel_main.Controls.Add(panel_cover);
            studentCount();
        }

        private void butexit_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.Show();
        }

        private void but_dash_Click(object sender, EventArgs e)
        {
            openChildForm(new Information_Teacher(user, pass));
            hideSubmenu();
        }
    }
}
