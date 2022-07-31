using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Parser3._0
{
    public struct Settings
    {
        public string Server;
        public string Database;
        public string Uid;
        public string Pwd;
        public string charset;
    }

    public class DBManager
    {
        static MySqlConnection con;
        static MySqlCommand cmd;

        static public Settings setting = new Settings();
        public static string conStr;

        public DBManager()
        {
            setting = BinConvert.ReadFromFile();
            con = new MySqlConnection();
            cmd = new MySqlCommand();
            cmd.Connection = con;
        }
              
        static public void connectTo()
        {                    
            conStr = "Server=" + setting.Server.ToString() + ";" + "Database=" + setting.Database.ToString() + ";"
            + "Uid=" + setting.Uid.ToString() + ";" + "Pwd=" + setting.Pwd.ToString() + ";";
            con.ConnectionString = conStr;
        }

        private List<Object> cmdExecute(string command)
        {
            var resRow = new List<Object>();
            cmd.CommandText = command;           
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {                   
                    resRow.Add(reader[i]);
                }
            }
            con.Close();
            return resRow;
        }

        public List<Object> ShowTables()
        {
            try
            {                
                return cmdExecute("SHOW TABLES");             
            }
            catch{ return null; }
        }

        public List<Object> ShowColumns(string tableName)
        {
            try
            {
                return cmdExecute("SELECT column_name FROM INFORMATION_SCHEMA.Columns WHERE TABLE_SCHEMA = '" + setting.Database + "'"
                    + " AND TABLE_NAME = '" + tableName + "'");                
            }
            catch{ return null; }            
        }
        
        public List<Object> DBFind(string tableName, string results, string column, string value)
        {
            try
            {
                return cmdExecute("SELECT " + results + " FROM " + tableName + " WHERE " + column + "=" + value);               
            }
            catch { return null; }
        }

        public void DBAdd(string tableName, string columns, string values)
        {
            try
            {                
                cmd.CommandText = "INSERT INTO " + tableName + "(" + columns + ") VALUES (" + values + ")";                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();          
            }
            catch
            {
                MessageBox.Show("Что-то не так, переделывай ");
            }
        }        
       
        public void ConOpen()
        {
            con.Open();
        }
        public void ConClose()
        {
            con.Close();
        }
       
        public async Task SelectAllAsync(string tableName, DataGridView dgv)
        {
            await Task.Run(() => SelectAll(tableName, dgv));
        }

        public void SelectAll(string tableName, DataGridView dgv)
        {
            try
            {
                var resRow = new List<List<object>>();
                cmd.CommandText = "SELECT * FROM " + tableName;
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                dgv.Columns.Clear();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dgv.Columns.Add("col" + i.ToString(), reader.GetName(i));
                }
                while (reader.Read())
                {
                    string[] s = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        s[i] = reader[i].ToString();
                    }
                    dgv.Rows.Add(s);
                }
                con.Close();
            }
            catch
            {
                MessageBox.Show("Что-то не так, переделывай ");  
            }
        }
    }
}