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
    }
}
