using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.BUS
{
    public class SubjectClass
    {
        DBconnect connect = new DBconnect();
        public string exeCount(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connect.getconnection);
            connect.openConnect();
            string count = command.ExecuteScalar().ToString();
            connect.closeConnect();
            return count;
        }
        public DataTable getuSbject(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public DataTable getListSUB(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool insertSubject(string sName, int couseID,string desc)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `subject`( `subject_Name`, `CourseId`, `Description`) VALUES (@sn,@cid,@desc)", connect.getconnection);
            //@cn,@ch,@desc
            command.Parameters.Add("@sn", MySqlDbType.VarChar).Value = sName;
            command.Parameters.Add("@cid", MySqlDbType.VarChar).Value = couseID;
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
        public List<int> getListSub(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            List<int> result = table.AsEnumerable().Select(n => n.Field<int>(0)).ToList();
            return result;
        }
        public List<string> getAllSUB(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            List<string> result = table.AsEnumerable().Select(n => n.Field<string>(0)).ToList();
            return result;
        }
        public bool Del_sub_stu_sco(int subID)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `sub_stu_sco` WHERE Subject_ID='" + subID + "'", connect.getconnection);
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
        public bool Del_teacher_subject(int subID)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `teacher_subject` WHERE Subject_ID='" + subID + "'", connect.getconnection);
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
        public bool Del_SUB(int subID)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `subject` WHERE Subject_ID='" + subID + "'", connect.getconnection);
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
        public bool Delete_SUBbyCourseID(int courseID)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `subject` WHERE CourseId='" + courseID + "'", connect.getconnection);
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
        public bool insertSub_Stu_Sco(int StdId,int Subject_ID)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `sub_stu_sco`( `StdId`, `Subject_ID`) VALUES (@stdid,@subid)", connect.getconnection);
            command.Parameters.Add("@stdid", MySqlDbType.VarChar).Value = StdId;
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
        public bool Update_SUB(int teacherID, int Subject_ID)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `teacher_subject` SET `Subject_ID`=@subid WHERE TeacherId=@teachid", connect.getconnection);
            command.Parameters.Add("@subid", MySqlDbType.Int32).Value = Subject_ID;
            command.Parameters.Add("@teachid", MySqlDbType.Int32).Value = teacherID;
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
        public DataTable searchSUB(string searchdata)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `subject` WHERE CONCAT(`subject_Name`) LIKE '%" + searchdata + "%'", connect.getconnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public bool update_SUBJECT(int id, string sub_name,int courseID, string desc)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `subject` SET `subject_Name`=@subname,`CourseId`=@courseid,`Description`=@desc WHERE Subject_ID=@subid", connect.getconnection);
            command.Parameters.Add("@subid", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@subname", MySqlDbType.VarChar).Value = sub_name;
            command.Parameters.Add("@courseid", MySqlDbType.Int32).Value = courseID;
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
    }
}
