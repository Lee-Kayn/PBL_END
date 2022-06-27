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
             DataGridView_course.DataSource = subject.getuSbject(new MySqlCommand("SELECT * FROM `subject`"));
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
            string courseID= DataGridView_course.CurrentRow.Cells[2].Value.ToString();
            string cName = course.exeCount("SELECT `CourseName` FROM `course` WHERE CourseId='" + courseID + "'");
            for(int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.GetItemText(comboBox1.Items[i])==cName)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }    
            }    
            textBox_description.Text = DataGridView_course.CurrentRow.Cells[3].Value.ToString();
        }
    }
}
