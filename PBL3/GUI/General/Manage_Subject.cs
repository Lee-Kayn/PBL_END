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

namespace PBL3.GUI.General
{
    public partial class Manage_Subject : Form
    {
        SubjectClass subject = new SubjectClass();
        CourseClass course = new CourseClass();
        ScoreClass ScoreClass = new ScoreClass();
        TeacherClass teacherClass = new TeacherClass();
        public Manage_Subject()
        {
            InitializeComponent();
        }

        private void Manage_Subject_Load(object sender, EventArgs e)
        {
            showData();
        }
        private void showData()
        {
             DataGridView_course.DataSource = subject.getuSbject(new MySqlCommand("SELECT `Subject_ID`, `subject_Name`,course.CourseName, subject.`Description` FROM `subject`,course WHERE course.CourseId=subject.CourseId"));
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
        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            textBox_id.Text = DataGridView_course.CurrentRow.Cells[0].Value.ToString();
            textBox_Cname.Text = DataGridView_course.CurrentRow.Cells[1].Value.ToString();
            string cName= DataGridView_course.CurrentRow.Cells[2].Value.ToString();
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.GetItemText(comboBox1.Items[i])==cName)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }    
            }    
            textBox_description.Text = DataGridView_course.CurrentRow.Cells[3].Value.ToString();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_id.Clear();
            textBox_Cname.Clear();
            textBox_description.Clear();
            comboBox1.SelectedIndex = 0;
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_course.DataSource=subject.searchSUB(textBox_search.Text);
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(textBox_id.Text);
            string sub_name = textBox_Cname.Text;
            string course_name=comboBox1.SelectedItem.ToString();
            int courseID = Convert.ToInt32(course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='"+course_name+"'"));
            string desc = textBox_description.Text;
            if(subject.update_SUBJECT(ID, sub_name, courseID, desc))
            {
                showData();
                MessageBox.Show("Student data update", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button_clear.PerformClick();
            }  
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (textBox_id.Text.Equals(""))
            {
                MessageBox.Show("Need Subject Id", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    foreach (DataGridViewRow row in DataGridView_course.SelectedRows)
                    {
                        int sub_ID = Convert.ToInt32(row.Cells[0].Value);
                        List<int> list_scoreid = ScoreClass.getList_ScoreID(new MySqlCommand("SELECT `ScoreId` FROM `sub_stu_sco` WHERE Subject_ID='" + sub_ID + "'"));
                        foreach(int scoreid in list_scoreid)
                        {
                            ScoreClass.deleteScore(scoreid);
                            ScoreClass.delete_sub_stu_sco(scoreid);
                        }
                        if(teacherClass.deleteTeacher_SUB(sub_ID))
                        {
                            subject.Del_SUB(sub_ID);
                        }    
                    }
                    showData();
                    MessageBox.Show("Subject Removed", "Remove Subject", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button_clear.PerformClick();
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message, "Removed Subject", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
