using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3
{
    public class TeacherClass
    {
        public int Flag = 0;
        CourseClass course = new CourseClass();
        DBconnect connect = new DBconnect();

        public bool insertTeacher(string fname, string lname, DateTime bdate, string gender, string phone, string address, byte[] img,int userID)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `teacher`(`Teacher_FN`, `Teacher_LN`, `Birthdate`, `Gender`, `Phone`, `Address`, `Photo`,`UserID`) VALUES(@fn, @ln, @bd, @gd, @ph, @adr, @img,@userID)", connect.getconnection);

            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;
            command.Parameters.Add("@userID", MySqlDbType.Int32).Value = userID;

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
        public DataTable getTeacherlist(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public string exeCount(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connect.getconnection);
            connect.openConnect();
            string count = command.ExecuteScalar().ToString();
            connect.closeConnect();
            return count;
        }
        public bool delUserID(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `user` WHERE `UserID`=@id", connect.getconnection);

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
        public bool del_teacher_subject(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `teacher_subject` WHERE `TeacherId`=@id", connect.getconnection);

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
        public string totalStudent()
        {
            return exeCount("SELECT COUNT(*) FROM teacher");
        }
        public string maleStudent()
        {
            return exeCount("SELECT COUNT(*) FROM teacher WHERE `Gender`='Male'");
        }
        public string femaleStudent()
        {
            return exeCount("SELECT COUNT(*) FROM teacher WHERE `Gender`='Female'");
        }
        public string getSubject()
        {
            return exeCount("Select Subject from `teacher`");
        }
        public string welcome(string userID)
        {
            return exeCount("SELECT Teacher_FN FROM `teacher` WHERE UserID='"+userID+"'") +" "+ exeCount("SELECT Teacher_LN FROM `teacher` WHERE UserID='"+userID +"'");
        }
        public string getUserID(string user,string pass)
        {
            return exeCount("Select UserID from user where username='" + user + "' and password='" + pass + "'");
        }
        public string getTeacherID(string userID)
        {
            return exeCount("SELECT `TeacherId` FROM `teacher` WHERE UserID='"+userID+"'");
        }
        public DataTable searchStudent(string searchdata)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `teacher` WHERE CONCAT(`Teacher_FN`,`Teacher_LN`,`Address`) LIKE '%" + searchdata + "%'", connect.getconnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool updateTeacher(int id, string fname, string lname, DateTime bdate, string gender, string phone, string address, byte[] img)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `teacher` SET `Teacher_FN`=@fn,`Teacher_LN`=@ln,`Birthdate`=@bd,`Gender`=@gd,`Phone`=@ph,`Address`=@adr,`Photo`=@img WHERE  `TeacherId`= @id", connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;

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
        public bool updateTeacher_Non(int id, string fname, string lname, DateTime bdate, string gender, string phone, string address)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `teacher` SET `Teacher_FN`=@fn,`Teacher_LN`=@ln,`Birthdate`=@bd,`Gender`=@gd,`Phone`=@ph,`Address`=@adr WHERE  `TeacherId`= @id", connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gd", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adr", MySqlDbType.VarChar).Value = address;

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
        public bool deleteTeacher(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `teacher` WHERE `TeacherId`=@id", connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            connect.openConnect();
            command.ExecuteNonQuery();
            return true;
            

        }
        public bool deleteTeacher_SUB(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `teacher_subject` WHERE `Subject_ID`=@id", connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            connect.openConnect();
            command.ExecuteNonQuery();
            return true;

        }
        public DataTable getList(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public bool insertTeacher_Subject(int TeacherID, int Subject_ID)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `teacher_subject`( `TeacherId`, `Subject_ID`) VALUES (@teacherid,@subid)", connect.getconnection);
            command.Parameters.Add("@teacherid", MySqlDbType.VarChar).Value = TeacherID;
            command.Parameters.Add("@subid", MySqlDbType.VarChar).Value = Subject_ID;
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
    }
}
