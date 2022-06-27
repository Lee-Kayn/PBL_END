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
    public partial class Score_STD : Form
    {
        CourseClass course = new CourseClass();
        ScoreClass score = new ScoreClass();
        TeacherClass teacher = new TeacherClass();
        StudentClass student = new StudentClass();
        string user, pass;
        public Score_STD(string user,string pass)
        {
            this.user = user;
            this.pass = pass;
            InitializeComponent();
        }

        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            comboBox_course.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `score` where studentId='" + student.STD_ID(user, pass) + "'"));
            comboBox_course.DisplayMember = "CourseName";
            comboBox_course.ValueMember = "CourseName";
            showScore();
        }
        public void showScore()
        {
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId,student.StdFirstName,student.StdLastName,score.CourseName,score.Score,score.Description FROM student INNER JOIN score ON score.StudentId=student.StdId where score.studentId='" + student.STD_ID(user,pass) + "'"));
        }
        private void DataGridView_course_Click(object sender, EventArgs e)
        {
            textBox_stdId.Text = DataGridView_score.CurrentRow.Cells[0].Value.ToString();
            comboBox_course.Text = DataGridView_score.CurrentRow.Cells[3].Value.ToString();
            textBox_score.Text = DataGridView_score.CurrentRow.Cells[4].Value.ToString();
            textBox_description.Text = DataGridView_score.CurrentRow.Cells[5].Value.ToString();
        }

    }
}
