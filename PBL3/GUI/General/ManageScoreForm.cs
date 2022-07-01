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
    public partial class ManageScoreForm : Form
    {
        CourseClass course = new CourseClass();
        ScoreClass score = new ScoreClass();
        TeacherClass teacher = new TeacherClass();
        SubjectClass subjectClass = new SubjectClass();
        List<int> pre_listSub = new List<int>();
        string user, pass;
        public ManageScoreForm(string user,string pass)
        {
            InitializeComponent();
            this.user = user;
            this.pass = pass;
        }

        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int count = Convert.ToInt32(teacher.exeCount("SELECT COUNT(*) FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            if (count > 0)
            {
                int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
                List<int> listSub = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
                pre_listSub = listSub;
                List<string> listCourse = new List<string>();
                foreach (int i in listSub)
                {
                    string sub_name = subjectClass.exeCount("SELECT `subject_Name` FROM `subject` WHERE Subject_ID='" + i + "'");
                    comboBox1.Items.Add("Sub_ID: " + i + ",Name = " + sub_name);
                    comboBox2.Items.Add("Sub_ID: " + i + ",Name = " + sub_name);
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + listSub[0] + "'"));
            }
        }
        public void showScore()
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + SubID + "'"));
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (txt_exam.Text == "" || textBox_score.Text == "" || txt_exam.Text == ""||comboBox1.SelectedIndex<0)
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int scoreID = Convert.ToInt32(txt_scroreID.Text);
                double exe = Convert.ToInt32(textBox_score.Text);
                string desc = textBox_description.Text;
                double exam= Convert.ToDouble(txt_exam.Text);
                double sum = exe * 0.4 + exam * 0.7;
                int select=Convert.ToInt32(comboBox1.SelectedIndex.ToString());
                int Sub_ID = pre_listSub[select];
                if (score.updateScore(scoreID,exe,exam,sum, desc)&& score.update_sub_stu_score(scoreID,Sub_ID))
                {
                        showScore();
                        button_clear.PerformClick();
                        MessageBox.Show("Score Edited Complete", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                        MessageBox.Show("Score not edit", "Update Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (textBox_stdId.Text == "")
            {
                MessageBox.Show("Field Error- we need scoreid", "Delete Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (DataGridViewRow row in DataGridView_score.SelectedRows)
                {
                    int scoreid = Convert.ToInt32(row.Cells[0].Value);
                    if (scoreid > 0)
                    {
                        if(score.deleteScore(scoreid))
                            if (score.delete_sub_stu_sco(scoreid))
                            {
                                continue;
                            }
                    }
                }
                showScore();
                MessageBox.Show("Score Removed", "Remove Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button_clear.PerformClick();

            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_stdId.Clear();
            textBox_score.Clear();
            textBox_description.Clear();
            txt_exam.Clear();
            txt_scroreID.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            int select = Convert.ToInt32(comboBox2.SelectedIndex.ToString());
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + pre_listSub[select] + "'"));
        }

        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            if(DataGridView_score.DataSource == null)
            {
                DataGridView_score.Enabled = false;
            }
            else
            {
                try
                {
                    textBox_stdId.Text = DataGridView_score.CurrentRow.Cells[1].Value.ToString();
                    txt_scroreID.Text = DataGridView_score.CurrentRow.Cells[0].Value.ToString();
                    textBox_score.Text = DataGridView_score.CurrentRow.Cells[3].Value.ToString();
                    txt_exam.Text = DataGridView_score.CurrentRow.Cells[4].Value.ToString();
                    textBox_description.Text = DataGridView_score.CurrentRow.Cells[6].Value.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Score Click", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        
    }
}
