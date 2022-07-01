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
    public partial class Subject_Form : Form
    {
        SubjectClass subject = new SubjectClass();
        CourseClass course = new CourseClass();
        TeacherClass teacher = new TeacherClass();
        public Subject_Form()
        {
            InitializeComponent();
        }

        private void Subject_Form_Load(object sender, EventArgs e)
        {
            showData();
            getcbbcourse();
            getcbbteacher_ID();
        }
        private void showData()
        {
            DataGridView_course.DataSource = subject.getuSbject(new MySqlCommand("SELECT * FROM `subject`"));
            
        }
        public void getcbbcourse()
        {
            foreach (string i in course.getListCourse(new MySqlCommand("SELECT `CourseName` FROM `course`")))
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;

        }
        public void getcbbteacher_ID()
        {
            foreach (int i in teacher.getcbbTeacher_ID())
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.SelectedIndex = 0;

        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Cname.Clear();
            textBox_description.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string courseName = comboBox1.SelectedItem.ToString();
            int courseID=Convert.ToInt32(course.exeCount("SELECT `CourseId` FROM `course` WHERE CourseName='"+courseName+"'"));
            int teacherid = Convert.ToInt32(comboBox2.SelectedItem.ToString());
            if (textBox_Cname.Text == "" || textBox_description.Text == "")
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string desc = textBox_description.Text;
                string subjectName = textBox_Cname.Text;
                if (subject.insertSubject(subjectName, courseID, desc))
                {
                    int subid = Convert.ToInt32(subject.exeCount("SELECT `Subject_ID` FROM `subject` WHERE subject_Name='"+subjectName+"' AND CourseId='"+courseID+"'"));
                    if(subject.insertSubject_teacher(teacherid,subid))
                    {
                        showData();
                        button_clear.PerformClick();
                        MessageBox.Show("New subject inserted", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }    

                }
                else
                {
                    MessageBox.Show("Course not insert", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
