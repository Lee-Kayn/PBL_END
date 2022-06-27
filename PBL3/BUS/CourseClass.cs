using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PBL3
{
    class CourseClass
    {
        DBconnect connect = new DBconnect();
        //create a function to insert course
        public bool insetCourse(string cName, DateTime start,DateTime end,string desc)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `course`(`CourseName`,`Date_Start`,`Date_End`, `Description`) VALUES (@cn,@start,@end,@desc)", connect.getconnection);
            //@cn,@ch,@desc
            command.Parameters.Add("@cn", MySqlDbType.VarChar).Value = cName;
            command.Parameters.Add("@start", MySqlDbType.Date).Value = start;
            command.Parameters.Add("@end", MySqlDbType.Date).Value = end;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = desc;
            connect.openConnect();
            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }
        }

        //create a function to get course list
        public DataTable getCourse(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public List<string> getListCourse(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            List<string> result = table.AsEnumerable().Select(n => n.Field<string>(0)).ToList();
            return result;
        }

        //create a update function for course edit
        public bool updateCourse(int id, string cName, string desc)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `course` SET`CourseName`=@c,`Description`=@desc WHERE  `CourseId`=@id", connect.getconnection);
            //@id,@cn,@ch,@desc
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@cn", MySqlDbType.VarChar).Value = cName;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = desc;
            connect.openConnect();
            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }
        }
        //Create a function to delete a course
        //we only need course id
        public bool deletCourse(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `course` WHERE `CourseId`=@id", connect.getconnection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            connect.openConnect();
            if (command.ExecuteNonQuery() == 1)
            {
                connect.closeConnect();
                return true;
            }
            else
            {
                connect.closeConnect();
                return false;
            }
        }
        public string exeCount(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connect.getconnection);
            connect.openConnect();
            string count = command.ExecuteScalar().ToString();
            connect.closeConnect();
            return count;
        }
    }
}
