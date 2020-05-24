using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace UI_WeatherAPP
{
    class Sql
    {
        private const string connstring = "";
        static private SqlConnection conn;
        static private SqlCommand query = null;

        private static SqlConnection ConnToDB()
        {
            SqlConnection retval = null;
            try
            {
                conn = new SqlConnection(connstring);
                conn.Open();
                retval = conn;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return retval;
        }

        public static DataTable FillDT(DataTable dt, string sql)
        {
            try
            {
                using(query = new SqlCommand(sql, ConnToDB()))
                {
                    query.Prepare();
                    SqlDataAdapter da = new SqlDataAdapter(sql, connstring);
                    da.Fill(dt);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
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
                sql += string.Format(" WHERE date BETWEEN '{0}' AND '{1}'", date1, date2);
                check = false;
            }
            //Select starting date only
            else if (date1.Length > 0 && date2.Length == 0)
            {
                sql += string.Format(" WHERE date => '{0}'", date1);
                check = false;
            }
            //Select ending date only
            else if (date2.Length > 0)
            {
                sql += string.Format(" WHERE date <= '{0}'", date2);
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
            sql += "ORDER BY date DESC";
            return sql;
        }
    }
}
