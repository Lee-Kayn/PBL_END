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

namespace PBL3.GUI.General
{
    public partial class Print_Subject : Form
    {
        CourseClass course = new CourseClass();
        DGVPrinter printer = new DGVPrinter();
        public Print_Subject()
        {
            InitializeComponent();
        }
        private void button_search_Click(object sender, EventArgs e)
        {
            DataGridView_student.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `subject` WHERE CONCAT(`CourseName`)LIKE '%" + textBox_search.Text + "%'"));
            textBox_search.Clear();
        }

        private void PrintCourseForm_Load(object sender, EventArgs e)
        {
            DataGridView_student.DataSource = course.getCourse(new MySqlCommand("SELECT * FROM `subject`"));
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            printer.Title = "Team88 Subject list";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Design & Developed by Team88";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(DataGridView_student);
        }
    }
}
