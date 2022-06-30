using DGVPrinterHelper;
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
    public partial class Print_STD : Form
    {
        ScoreClass score = new ScoreClass();
        DGVPrinter printer = new DGVPrinter();
        StudentClass student = new StudentClass();
        UserClass userClass = new UserClass();  
        string user, pass;
        public Print_STD(string user,string pass)
        {
            this.user = user;
            this.pass=pass;
            InitializeComponent();
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            //We need DGVprinter helper for print pdf file
            printer.Title = "Team88 Student score list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
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
            showScore();
        }
        //to show score list
        public void showScore()
        {
            string userID=userClass.exeCount("SELECT `UserID` FROM `user` WHERE username='"+user+"' AND password='"+pass+"'");
            string STD_ID = student.exeCount("SELECT `StdId` FROM `student` WHERE UserID='" + userID + "'");
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT student.`StdId`, `StdFirstName`, `StdLastName`, `Birthdate`, `Gender`, `Phone`,sub_stu_sco.Subject_ID,subject.subject_Name,score.Exercise,score.Exam,score.Summary,score.Description FROM `student`,score,sub_stu_sco,subject WHERE score.ScoreId=sub_stu_sco.ScoreId AND sub_stu_sco.StdId='"+STD_ID+"' AND student.StdId='"+STD_ID+"' AND subject.Subject_ID=sub_stu_sco.Subject_ID"));
        }

    }
}
