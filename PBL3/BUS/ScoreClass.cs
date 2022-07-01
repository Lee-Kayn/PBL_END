using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3
{
    class ScoreClass
    {
        DBconnect connect = new DBconnect();

        public bool insertScore(int scoreid, double exc,double Exam,double Sum, string desc)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `score`(`ScoreId`, `Exercise`, `Exam`, `Summary`, `Description`) VALUES (@scoreid,@exe,@Exam,@Sum,@desc)", connect.getconnection);

            command.Parameters.Add("@scoreid", MySqlDbType.Int32).Value = scoreid;
            command.Parameters.Add("@exe", MySqlDbType.Float).Value = exc;
            command.Parameters.Add("@Exam", MySqlDbType.Float).Value = Exam;
            command.Parameters.Add("@Sum", MySqlDbType.Float).Value = Sum;
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

        public DataTable getList(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool checkScore(int stdId, string Sub_ID)
        {
            DataTable table = getList(new MySqlCommand("SELECT * FROM `sub_stu_sco` WHERE `StdId`= " + stdId + " AND `Subject_ID`= '" + Sub_ID + "'"));
            if (table.Rows.Count > 0)
            { return true; }
            else
            { return false; }
        }

        public bool updateScore(int scoreid, double exe, double exam,double sum, string desc)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `score` SET `Exercise`=@exe,`Exam`=@exam,`Summary`=@sum,`Description`=@desc WHERE ScoreId='"+scoreid+"'", connect.getconnection);
            command.Parameters.Add("@exe", MySqlDbType.Double).Value = exe;
            command.Parameters.Add("@exam", MySqlDbType.Double).Value = exam;
            command.Parameters.Add("@sum", MySqlDbType.VarChar).Value = sum;
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
        public bool update_sub_stu_score(int scoreid,int Sub_ID)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `sub_stu_sco` SET `Subject_ID`=@sub_id WHERE ScoreId='"+scoreid+"'", connect.getconnection);
            command.Parameters.Add("@sub_id", MySqlDbType.Int32).Value = Sub_ID;
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

        public bool deleteScore(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `score` WHERE `ScoreId`=@id ", connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            connect.openConnect();
            command.ExecuteNonQuery();
            connect.closeConnect();
            return true;
        }
        public bool delete_sub_stu_sco(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `sub_stu_sco` WHERE `ScoreId`=@id ", connect.getconnection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            connect.openConnect();
            command.ExecuteNonQuery();
            connect.closeConnect();
            return true;
        }
        public List<int> getList_ScoreID(MySqlCommand command)
        {
            command.Connection = connect.getconnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            List<int> result = table.AsEnumerable().Select(n => n.Field<int>(0)).ToList();
            return result;
        }
    }
}
