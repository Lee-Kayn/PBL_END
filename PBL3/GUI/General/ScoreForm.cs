using MySql.Data.MySqlClient;
using PBL3.BUS;
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
    public partial class ScoreForm : Form
    {
        CourseClass course = new CourseClass();
        StudentClass student = new StudentClass();
        ScoreClass score = new ScoreClass();
        TeacherClass teacher = new TeacherClass();
        SubjectClass subjectClass = new SubjectClass();
        string user, pass;
        public ScoreForm(string user,string pass)
        {
            InitializeComponent();
            this.user = user;
            this.pass = pass;
        }
        //create a function to show data on datagridview score
        private void showScoe()
        {
             DataGridView_student.DataSource = score.getList(new MySqlCommand("SELECT * FROM `sub_stu_sco`"));
        }

        private void ScoreForm_Load(object sender, EventArgs e)
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            List<int> listSub = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            List<string> listCourse = new List<string>();
            foreach (int i in listSub)
            {
                string courseID = subjectClass.exeCount("SELECT `CourseId` FROM `subject` WHERE Subject_ID='" + i + "'");
                string couerseName = course.exeCount("SELECT `CourseName` FROM `course` WHERE CourseId='" + courseID + "'");
                listCourse.Add(couerseName);
                comboBox_course.Items.Add(couerseName);
            }
            comboBox_course.SelectedIndex = 0;
            DataGridView_student.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + SubID+"'"));
            //comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course` where CourseName='" + teacher.getSubject() + "'"));
            //comboBox_course.DisplayMember = "CourseName";
            //comboBox_course.ValueMember = "CourseName";
            // to show data on score datagridview

            //To Display the student list on Datagridview
            //DataGridView_student.DataSource = student.getList(new MySqlCommand("SELECT `StdId`,`StdFirstName`,`StdLastName` FROM `student` INNER JOIN score ON score.StudentId=student.StdId where score.CourseName='" + teacher.getSubject() + "'"));
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_stdId.Text == "" || textBox_score.Text == ""||comboBox_course.SelectedIndex<0||comboBox1.SelectedIndex<0)
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int stdId = Convert.ToInt32(textBox_stdId.Text);
                string cName = comboBox_course.Text;
                string course_ID = course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + cName + "'");
                double exc = Convert.ToInt32(textBox_score.Text);
                double Exam= Convert.ToInt32(textBox1.Text);
                double Sum = exc * 0.3 + Exam * 0.7;
                string sub_name = comboBox1.Text;
                string Sub_ID = subjectClass.exeCount("SELECT `Subject_ID` FROM `subject` WHERE CourseId='"+course_ID+"'");
                string desc = textBox_description.Text;
                if (student.Check_ID(stdId) !=0)
                {
                    if (!score.checkScore(stdId, Sub_ID))
                    {
                        subjectClass.insertSub_Stu_Sco(stdId, Convert.ToInt32(Sub_ID));
                        if (score.insertScore(stdId, exc,Exam,Sum, desc))
                        {
                            showScoe();
                            button_clear.PerformClick();
                            MessageBox.Show("New score added", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("Score not added", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The score for this course are alerady exists", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("StudentId does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            showScoe();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_stdId.Clear();
            textBox_score.Clear();
            comboBox_course.SelectedIndex = 0;
            textBox_description.Clear();
        }

        private void DataGridView_student_Click(object sender, EventArgs e)
        {
            textBox_stdId.Text = DataGridView_student.CurrentRow.Cells[0].Value.ToString();
        }

        private void button_sStudent_Click(object sender, EventArgs e)
        {
            DataGridView_student.DataSource = student.getList(new MySqlCommand("SELECT `StdId`,`StdFirstName`,`StdLastName` FROM `student`"));
        }

        private void comboBox_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string name_course = comboBox_course.SelectedItem.ToString();
            string ID_course = course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + name_course + "'");
            foreach (string i in course.getListCourse(new MySqlCommand("SELECT `Subject_Name` FROM `subject` WHERE CourseId='" + ID_course + "'")))
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button_sScore_Click(object sender, EventArgs e)
        {
            showScoe();
        }
    }
}
