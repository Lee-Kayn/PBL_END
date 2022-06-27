using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace PBL3
{
    public partial class CourseForm : Form
    {
        CourseClass course = new CourseClass();
        public CourseForm()
        {
            InitializeComponent();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_Cname.Text == "" || textBox_description.Text == "")
            {
                MessageBox.Show("Need Course data", "Field Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string cName = textBox_Cname.Text;
                string desc = textBox_description.Text;
                DateTime start = dateTimePicker1.Value;
                DateTime end = dateTimePicker2.Value;


                if (course.insetCourse(cName,start,end,desc))
                {
                    showData();
                    button_clear.PerformClick();
                    MessageBox.Show("New course inserted", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Course not insert", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_Cname.Clear();
            textBox_description.Clear();
        }

        private void CourseForm_Load(object sender, EventArgs e)
        {
            showData();
        }
        private void showData()
        {
            //to show course list on datagridview
            DataGridView_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `course`"));
        }
    }
}
