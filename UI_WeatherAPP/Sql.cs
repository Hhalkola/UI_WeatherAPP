using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Windows.UI.Popups;
using MySql.Data.MySqlClient;

namespace UI_WeatherAPP
{
    class Sql
    {
        static private string connstring = ReadTxt();

        private static MySqlConnection ConnToDB()
        {
            MySqlConnection con = null;
            try
            {
                con = new MySqlConnection(connstring);
                con.Open();
            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                _ = ms.ShowAsync();
            }
            return con;
        }

        public static DataTable FillDT(DataTable dt, string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, ConnToDB());
                MySqlDataAdapter da = new MySqlDataAdapter(sql, ConnToDB());
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                _ = ms.ShowAsync();
            }
            return dt;
        }
        

        public static string Query(string date1, string date2, string tempmin, string tempmax)
        {
            bool check = true;
            string sql = "SELECT * FROM weather";

            //Select *
            if (date1.Equals("") && date2.Equals("") && tempmin.Equals("") && tempmax.Equals(""))
            {
                return sql;
            }
            //Select both dates
            if (date1.Length > 0 && date2.Length > 0)
            {
                sql += string.Format(" WHERE datevalue BETWEEN '{0}' AND '{1}'", date1, date2);
                check = false;
            }
            //Select starting date only
            else if (date1.Length > 0 && date2.Length == 0)
            {
                sql += string.Format(" WHERE datevalue >= '{0}'", date1);
                check = false;
            }
            //Select ending date only
            else if (date2.Length > 0)
            {
                sql += string.Format(" WHERE datevalue <= '{0}'", date2);
                check = false;
            }
            //Check do we need WHERE or AND next
            if (tempmin.Length > 0 && tempmax.Length > 0)
            {
                if (check)
                {
                    sql += string.Format(" WHERE temperature BETWEEN '{0}' AND '{1}'", tempmin, tempmax);
                }
                else
                {
                    sql += string.Format(" AND temperature BETWEEN '{0}' AND '{1}'", tempmin, tempmax);
                }

            }
            else if (tempmin.Length > 0 && tempmax.Equals(""))
            {
                sql += string.Format(" WHERE temperature => '{0}'", tempmin);
            }
            else if (tempmin.Length == 0 && tempmax.Length > 0)
            {
                sql += string.Format(" WHERE temperature <= '{0}'", tempmax);
            }
            sql += "ORDER BY id DESC";
            return sql;
        }

        private static string ReadTxt()
        {
            string s = "";
            try
            {
               using (StreamReader sr = new StreamReader("TextFile1.txt"))
                {
                    s = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {

                MessageDialog ms = new MessageDialog(e.Message);
                _ = ms.ShowAsync();
            }
            return s;
        }
    }
}
