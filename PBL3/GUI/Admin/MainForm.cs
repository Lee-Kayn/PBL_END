using MySql.Data.MySqlClient;
using PBL3.BUS;
using PBL3.GUI.General;
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
    public partial class MainForm : Form
    {
        StudentClass student = new StudentClass();
        CourseClass course = new CourseClass();
        SubjectClass subjectClass = new SubjectClass();
        ScoreClass scoreClass =new ScoreClass();
        int Flag = 0;
        int COUNT;
        public MainForm()
        {
            InitializeComponent();
            customizeDesign();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            List<int> list = course.getcourseID();
            int count = 0;
            foreach(int i in list)
            {
                DateTime now=DateTime.Now;
                DateTime end = course.getTimeEND(i);
                if(now>end)
                {
                    TimeSpan value = now-end;
                    int Day = value.Days;
                    if(Day<10)
                    {
                        count++;
                    }
                    else
                    {
                        
                        List<int> sub_Id = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID`FROM `subject` WHERE CourseId='"+i+"'"));
                        foreach(int k in sub_Id)
                        {
                            List<int> listscore_ID = scoreClass.getList_ScoreID(new MySqlCommand("SELECT `ScoreId` FROM `sub_stu_sco` WHERE Subject_ID='"+k+"'"));
                            foreach(int j in listscore_ID)
                            {
                                scoreClass.deleteScore(j);
                            }    
                            subjectClass.Del_sub_stu_sco(k);
                            subjectClass.Del_teacher_subject(k);
                        }
                        subjectClass.Delete_SUBbyCourseID(i);
                        course.deletCourse(i);
                    }    
                }    
            }    
            studentCount();
            foreach(string i in subjectClass.getAllSUB(new MySqlCommand("SELECT `subject_Name` FROM `subject`")))
            {
                comboBox_course.Items.Add(i);
            }
            this.COUNT = count;
            if (COUNT > 0)
            {
                MessageBox.Show("There are " + COUNT + " courses that are overdue. Please check your data and back it up before the course automatically deletes", "Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void studentCount()
        {
            label_totalStd.Text = "Total Students : " + student.totalStudent();
            label_maleStd.Text = "Male : " + student.maleStudent();
            label_femaleStd.Text = "Female : " + student.femaleStudent();

        }


        private void customizeDesign()
        {
            panel_stdsubmenu.Visible = false;
            panel_courseSubmenu.Visible = false;
            panel_scoreSubmenu.Visible = false;
            panel_teacher.Visible = false;
        }

        private void hideSubmenu()
        {
            if (panel_stdsubmenu.Visible == true)
                panel_stdsubmenu.Visible = false;
            if (panel_courseSubmenu.Visible == true)
                panel_courseSubmenu.Visible = false;
            if (panel_scoreSubmenu.Visible == true)
                panel_scoreSubmenu.Visible = false;
            if (panel_teacher.Visible == true)
                panel_teacher.Visible = false;
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
            Flag = 0;
        }
        #region StdSubmenu
        private void button_registration_Click(object sender, EventArgs e)
        {
            openChildForm(new RegisterForm(Flag));
            hideSubmenu();
            
        }

        private void button_manageStd_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageStudentForm());
            hideSubmenu();
        }

        private void button_stdPrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintStudent());
            hideSubmenu();
        }

        #endregion StdSubmenu
        private void button_course_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_courseSubmenu);
        }
        #region CourseSubmenu
        private void button_newCourse_Click(object sender, EventArgs e)
        {
            openChildForm(new CourseForm());
            hideSubmenu();
        }

        private void button_manageCourse_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageCourseForm());
            hideSubmenu();
        }

        private void button_coursePrint_Click(object sender, EventArgs e)
        {
            openChildForm(new PrintCourseForm());
            hideSubmenu();
        }
        #endregion CourseSubmenu

        private void button_score_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_scoreSubmenu);
        }
        #region ScoreSubmenu
        private void button_newScore_Click(object sender, EventArgs e)
        {
            openChildForm(new Subject_Form());
            hideSubmenu();
        }

        private void button_manageScore_Click(object sender, EventArgs e)
        {
            openChildForm(new Manage_Subject());
            hideSubmenu();
        }

        private void button_scorePrint_Click(object sender, EventArgs e)
        {
            openChildForm(new Print_Subject());
            hideSubmenu();
        }


        #endregion ScoreSubmenu

        private void but_teacher_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_teacher);
        }
        #region Teacher
        private void button3_Click(object sender, EventArgs e)
        {
            openChildForm(new Register_Teacher());
            hideSubmenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new ManageTeacher());
            hideSubmenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Printer_Teacher());
            hideSubmenu();
        }
        #endregion Teacher

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
            string sub_name=comboBox_course.Text;
            string sub_ID = subjectClass.exeCount("SELECT `Subject_ID` FROM `subject` WHERE subject_Name='" + sub_name + "'");
            label_cmale.Text = "Male : " + student.exeCount("SELECT COUNT(*) FROM `student`,sub_stu_sco WHERE student.StdId=sub_stu_sco.StdId AND sub_stu_sco.Subject_ID='" + sub_ID+ "' AND Gender='Male'");
            label_cfemale.Text = "Female : " + student.exeCount("SELECT COUNT(*) FROM `student`,sub_stu_sco WHERE student.StdId=sub_stu_sco.StdId AND sub_stu_sco.Subject_ID='" + sub_ID + "' AND Gender='Female'");
        }

        private void but_exit_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.Show();
        }

        private void but_dash_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            panel_main.Controls.Add(panel_cover);
            studentCount();
        }

    }
}