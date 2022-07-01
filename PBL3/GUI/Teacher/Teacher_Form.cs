using MySql.Data.MySqlClient;
using PBL3.BUS;
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
        SubjectClass subjectClass = new SubjectClass();
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
            string userID=teacher.getUserID(user,pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            label_user.Text = teacher.welcome(userID);
            List<int> listSub = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='"+TeacherID+"'"));
            foreach(int i in listSub)
            {
                string Sub_Name = subjectClass.exeCount("SELECT `subject_Name` FROM `subject` WHERE Subject_ID='"+i+"'");
                comboBox_course.Items.Add(Sub_Name);
            }    
         
        }


        private void customizeDesign()
        {
            panel_scoreSubmenu.Visible = false;
        }

        private void hideSubmenu()
        {
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
            openChildForm(new ScoreForm(user,pass));
            hideSubmenu();
        }

        private void button_manageScore_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageScoreForm(user,pass));
            hideSubmenu();
        }

        private void button_scorePrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintScoreForm(user,pass));
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
            string sub_name = comboBox_course.Text;
            string sub_ID = subjectClass.exeCount("SELECT `Subject_ID` FROM `subject` WHERE subject_Name='" + sub_name + "'");
            label_cmale.Text = "Male : " + student.exeCount("SELECT COUNT(*) FROM `student`,sub_stu_sco WHERE student.StdId=sub_stu_sco.StdId AND sub_stu_sco.Subject_ID='" + sub_ID + "' AND Gender='Male'");
            label_cfemale.Text = "Female : " + student.exeCount("SELECT COUNT(*) FROM `student`,sub_stu_sco WHERE student.StdId=sub_stu_sco.StdId AND sub_stu_sco.Subject_ID='" + sub_ID + "' AND Gender='Female'");
        }

        private void but_exit_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            panel_main.Controls.Add(panel_cover);
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
