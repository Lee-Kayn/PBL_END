using MySql.Data.MySqlClient;
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
        int flag;
        public ManageScoreForm(int Flag)
        {
            InitializeComponent();
            flag= Flag;
        }

        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            //populate the combobox with courses name
            if(flag!=1)
            {
                comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
                comboBox_course.DisplayMember = "CourseName";
                comboBox_course.ValueMember = "CourseName";
            }
            else
            {
                comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course` where CourseName='"+teacher.getSubject()+"'"));
                comboBox_course.DisplayMember = "CourseName";
                comboBox_course.ValueMember = "CourseName";
            }
            
            // to show score data on datagridview
            showScore();
        }
        public void showScore()
        {
            if(flag!=1)
                DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId,student.StdFirstName,student.StdLastName,score.CourseName,score.Score,score.Description FROM student INNER JOIN score ON score.StudentId=student.StdId "));
            else
                DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId,student.StdFirstName,student.StdLastName,score.CourseName,score.Score,score.Description FROM student INNER JOIN score ON score.StudentId=student.StdId where score.CourseName='"+teacher.getSubject()+"'"));
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (textBox_stdId.Text == "" || textBox_score.Text == "")
            {
                MessageBox.Show("Need score data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int stdId = Convert.ToInt32(textBox_stdId.Text);
                string cName = comboBox_course.Text;
                double scor = Convert.ToInt32(textBox_score.Text);
                string desc = textBox_description.Text;
              
                if (score.updateScore(stdId,cName,scor, desc))
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
                MessageBox.Show("Field Error- we need student id", "Delete Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (DataGridViewRow row in DataGridView_score.SelectedRows)
                {
                    int rowId = Convert.ToInt32(row.Cells[0].Value);
                    string rowCourse = row.Cells[3].Value.ToString();
                    if (rowId > 0)
                    {
                        if (score.deleteScore(rowId,rowCourse))
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
            textBox_search.Clear();
        }

        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            try
            {
                textBox_stdId.Text = DataGridView_score.CurrentRow.Cells[0].Value.ToString();
                comboBox_course.Text = DataGridView_score.CurrentRow.Cells[3].Value.ToString();
                textBox_score.Text = DataGridView_score.CurrentRow.Cells[4].Value.ToString();
                textBox_description.Text = DataGridView_score.CurrentRow.Cells[5].Value.ToString();
            } catch
            {
                MessageBox.Show("None Data", "Score Click", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId, student.StdFirstName, student.StdLastName, score.CourseName, score.Score, score.Description FROM student INNER JOIN score ON score.StudentId = student.StdId WHERE CONCAT(student.StdFirstName, student.StdLastName, score.CourseName)LIKE '%"+textBox_search.Text+"%'"));
            
        }
    }
}
