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
    public partial class Printer_Teacher : Form
    {
        StudentClass student = new StudentClass();
        DGVPrinter printer = new DGVPrinter();

        public Printer_Teacher()
        {
            InitializeComponent();
        }

        private void PrintTeacher_Load(object sender, EventArgs e)
        {
            showData(new MySqlCommand("SELECT * FROM `teacher`"));
        }

        // create a function to show the student list in datagridview
        public void showData(MySqlCommand command)
        {
            DataGridView_student.ReadOnly = true;
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            DataGridView_student.DataSource = student.getList(command);
            imageColumn = (DataGridViewImageColumn)DataGridView_student.Columns[7];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            //check the radio button
            string selectQuery;
            if (radioButton_all.Checked)
            {
                selectQuery = "SELECT* FROM `teacher`";
            }
            else if (radioButton_male.Checked)
            {
                selectQuery = "SELECT * FROM `teacher` WHERE `Gender`='Male'";
            }
            else
            {
                selectQuery = "SELECT * FROM `teacher` WHERE `Gender`='Female'";
            }
            showData(new MySqlCommand(selectQuery));
        }

        private void button_print_Click_1(object sender, EventArgs e)
        {
            printer.Title = "Team88 Programing School Teacher list";
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
