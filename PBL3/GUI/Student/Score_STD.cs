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
    public partial class Score_STD : Form
    {
        CourseClass course = new CourseClass();
        ScoreClass score = new ScoreClass();
        TeacherClass teacher = new TeacherClass();
        StudentClass student = new StudentClass();
        UserClass userClass = new UserClass();
        string user, pass,ID;
        public Score_STD(string user,string pass)
        {
            this.user = user;
            this.pass = pass;
            InitializeComponent();
        }

        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            string userID = userClass.exeCount("SELECT `UserID` FROM `user` WHERE username='" + user + "'AND password='" + pass + "'");
            string StdID = student.exeCount("SELECT `StdId` FROM `student` WHERE UserID='" + userID + "'");
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.StdId='" + StdID + "'"));
            ID = userID;
        }
        public void showScore()
        {
            string userID = userClass.exeCount("SELECT `UserID` FROM `user` WHERE username='" + user + "'AND password='" + pass + "'");
            string StdID = student.exeCount("SELECT `StdId` FROM `student` WHERE UserID='" + userID + "'");
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.StdId='"+StdID+"'"));
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
