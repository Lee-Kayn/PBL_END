using DGVPrinterHelper;
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
    public partial class Print_STD : Form
    {
        ScoreClass score = new ScoreClass();
        DGVPrinter printer = new DGVPrinter();
        StudentClass student = new StudentClass();
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
            DataGridView_score.DataSource = score.getList(new MySqlCommand("SELECT score.StudentId,student.StdFirstName,student.StdLastName,score.CourseName,score.Score,score.Description FROM student INNER JOIN score ON score.StudentId=student.StdId where score.studentId='" + student.STD_ID(user, pass) + "'"));
        }

    }
}
