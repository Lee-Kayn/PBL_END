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
using DGVPrinterHelper;
using PBL3.BUS;

namespace PBL3
{
    public partial class PrintScoreForm : Form
    {
        DGVPrinter printer = new DGVPrinter();
        ScoreClass score = new ScoreClass();
        TeacherClass teacher = new TeacherClass();
        SubjectClass subjectClass = new SubjectClass();
        List<int> pre_listSub = new List<int>();
        string user, pass;
        public PrintScoreForm(string user,string pass)
        {
            InitializeComponent();
            this.user = user;
            this.pass = pass;
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            //We need DGVprinter helper for print pdf file
            printer.Title = "Team88 Student score list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Design & Developed by Team88";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_score);
        }

        private void PrintScoreForm_Load(object sender, EventArgs e)
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            List<int> listSub = subjectClass.getListSub(new MySqlCommand("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            pre_listSub = listSub;
            List<string> listCourse = new List<string>();
            foreach (int i in listSub)
            {
                string sub_name = subjectClass.exeCount("SELECT `subject_Name` FROM `subject` WHERE Subject_ID='" + i + "'");
                comboBox1.Items.Add("Sub_ID: " + i + ",Name = " + sub_name);
            }
            comboBox1.SelectedIndex = 0;
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + listSub[0] + "'"));
        }
        //to show score list
        public void showScore()
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + SubID + "'"));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userID = teacher.getUserID(user, pass);
            int TeacherID = Convert.ToInt32(teacher.getTeacherID(userID));
            int SubID = Convert.ToInt32(teacher.exeCount("SELECT `Subject_ID` FROM `teacher_subject` WHERE TeacherId='" + TeacherID + "'"));
            int select = Convert.ToInt32(comboBox1.SelectedIndex.ToString());
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT sub_stu_sco.`ScoreId`, `StdId`, `Subject_ID`,score.Exercise,score.Exam,score.Summary,score.Description FROM `sub_stu_sco`,score WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.Subject_ID='" + pre_listSub[select] + "'"));
        }
    }
}
