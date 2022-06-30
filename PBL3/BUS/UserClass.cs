using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.BUS
{
    public class UserClass
    {
        DBconnect connect = new DBconnect();
        public bool Add(string user,string pass,string role)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `user`(`Username`, `Password`, `Role`) VALUES (@user,@pass,@role)", connect.getconnection);
            command.Parameters.Add("@user", MySqlDbType.VarChar).Value = user;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = pass;
            command.Parameters.Add("@role", MySqlDbType.VarChar).Value = role;
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
        public string getUsername(int ID)
        {
            return exeCount("SELECT `username`FROM `user` WHERE UserID = '" + ID + "'");
        }
        public string getPassword(int ID)
        {
            return exeCount("SELECT `password`FROM `user` WHERE UserID = '" + ID + "'");
        }

        public bool update_user(int ID, string username,string pass)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `user` SET `username`=@user,`password`=@pass WHERE  `UserID`= @id", connect.getconnection);
            command.Parameters.Add("@user", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = pass;
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = ID;
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
        public bool delete_user(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `user` WHERE UserID=@id", connect.getconnection);

            //@id
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
    }
}
