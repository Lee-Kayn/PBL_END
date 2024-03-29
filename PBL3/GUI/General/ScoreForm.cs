﻿using MySql.Data.MySqlClient;
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
        private void showScoe()
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int count = Convert.ToInt32(teacher.exeCount("SELECT COUNT(*) FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            if (count > 0)
            {
                int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
                DataGridView_student.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + SubID + "'"));
            }    
                
        }

        private void ScoreForm_Load(object sender, EventArgs e)
        {
            
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int count = Convert.ToInt32(teacher.exeCount("SELECT COUNT(*) FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            if(count > 0)
            {
                int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
                List<int> listSub = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
                List<string> listCourse = new List<string>();
                foreach (int i in listSub)
                {
                    string courseID = subjectClass.exeCount("SELECT `CourseId` FROM `subject` WHERE Subject_ID='" + i + "'");
                    string couerseName;
                    int C = Convert.ToInt32(course.exeCount("SELECT COUNT(*) FROM `course` WHERE CourseId='" + courseID + "' AND Date_End>CURRENT_TIMESTAMP()"));
                    if(C>0)
                    {
                        couerseName = course.exeCount("SELECT `CourseName` FROM `course` WHERE CourseId='" + courseID + "'");
                        listCourse.Add(couerseName);
                        comboBox_course.Items.Add(couerseName);
                    }
                }
                comboBox_course.SelectedIndex = 0;
                DataGridView_student.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + SubID + "'"));
            }    
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_stdId.Text == "" ||comboBox_course.SelectedIndex<0||comboBox1.SelectedIndex<0)
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int stdId = Convert.ToInt32(textBox_stdId.Text);
                string cName = comboBox_course.Text;
                string course_ID = course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='" + cName + "'");
                double exc, Exam;
                if(textBox_score.Text!="")
                {
                     exc = Convert.ToInt32(textBox_score.Text);
                }
                else
                {
                    exc = 0;
                }
                if (textBox_score.Text != "")
                {
                     Exam = Convert.ToInt32(textBox1.Text);
                }
                else
                {
                    Exam=0;
                }
                double Sum=0;
                if(exc!=0|| Exam!=0)
                    Sum = exc * 0.3 + Exam * 0.7;
                string sub_name = comboBox1.Text;
                string Sub_ID = subjectClass.exeCount("SELECT `Subject_ID` FROM `subject` WHERE CourseId='"+course_ID+"'");
                string desc = textBox_description.Text;
                if (student.Check_ID(stdId) !=0)
                {
                    if (!score.checkScore(stdId, Sub_ID))
                    {
                        subjectClass.insertSub_Stu_Sco(stdId, Convert.ToInt32(Sub_ID));
                        int scoreID = Convert.ToInt32(student.exeCount("SELECT `ScoreId`FROM `sub_stu_sco` WHERE StdId='" + stdId + "' AND Subject_ID='" + Sub_ID + "'"));
                        if (score.insertScore(scoreID, exc,Exam,Sum, desc))
                        {
                            showScoe();
                            button_clear.PerformClick();
                            if(Sum!=0)
                            {
                                MessageBox.Show("New score added", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }  
                            else
                            {
                                MessageBox.Show("New student added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }    

                        }
                        else
                        {
                            MessageBox.Show("Score not added", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The score for this subject are alerady exists", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if(comboBox_course.Items.Count>0)
                comboBox_course.SelectedIndex = 0;
            textBox_description.Clear();
            textBox1.Clear();
        }

        private void DataGridView_student_Click(object sender, EventArgs e)
        {
            try
            {
                textBox_stdId.Text = DataGridView_student.CurrentRow.Cells[0].Value.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Score Click", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
