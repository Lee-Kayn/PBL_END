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
    public partial class ManageCourseForm : Form
    {
        CourseClass course = new CourseClass();
        TeacherClass teacher = new TeacherClass();
        SubjectClass subjectClass = new SubjectClass();
        ScoreClass scoreClass = new ScoreClass();
        public ManageCourseForm()
        {
            InitializeComponent();
        }

        private void ManageCourseForm_Load(object sender, EventArgs e)
        {
            showData();

        }
        private void showData()
        {
            if(teacher.Flag==0)
                DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
            else 
                DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course` where CourseName='"+teacher.getSubject().ToString()+"'"));
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Cname.Clear();
            textBox_description.Clear();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (textBox_Cname.Text == ""|| textBox_id.Text.Equals(""))
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            { 
                int id = Convert.ToInt32(textBox_id.Text);
                string cName = textBox_Cname.Text;
                string desc = textBox_description.Text;


                if (course.updateCourse(id, cName, desc))
                {
                    showData();
                    button_clear.PerformClick();
                    MessageBox.Show("course update successfuly", "Update Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Error-Course not Edit", "Update Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (textBox_id.Text.Equals(""))
            {
                MessageBox.Show("Need Course Id", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    foreach (DataGridViewRow row in DataGridView_course.SelectedRows)
                    {
                        int i = Convert.ToInt32(row.Cells[0].Value);
                        List<int> sub_Id = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID`FROM `subject` WHERE CourseId='" + i + "'"));
                        foreach (int k in sub_Id)
                        {
                            List<int> listscore_ID = scoreClass.getList_ScoreID(new MySqlCommand("SELECT `ScoreId` FROM `sub_stu_sco` WHERE Subject_ID='" + k + "'"));
                            foreach (int j in listscore_ID)
                            {
                                scoreClass.deleteScore(j);
                            }
                            subjectClass.Del_sub_stu_sco(k);
                            subjectClass.Del_teacher_subject(k);
                        }
                        subjectClass.Delete_SUBbyCourseID(i);
                        course.deletCourse(i);
                    }
                    showData();
                    MessageBox.Show("Course Removed", "Remove Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button_clear.PerformClick();
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Removed Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_course.CurrentRow.Cells[0].Value.ToString();
            textBox_Cname.Text = DataGridView_course.CurrentRow.Cells[1].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(DataGridView_course.CurrentRow.Cells[2].Value.ToString());
            dateTimePicker2.Value = Convert.ToDateTime(DataGridView_course.CurrentRow.Cells[3].Value.ToString());
            textBox_description.Text = DataGridView_course.CurrentRow.Cells[4].Value.ToString();
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course` WHERE CONCAT(`CourseName`)LIKE '%"+textBox_search.Text+"%'"));
            textBox_search.Clear();
        }
    }
}
