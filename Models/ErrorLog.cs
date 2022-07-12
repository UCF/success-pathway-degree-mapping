using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapping.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string LogSource { get; set; }
        public string LogMessage { get; set; }
        public string Exception { get; set; }

        public ErrorLog()
        { 
        
        }

        public static string Get(int id)
        {
            string message = string.Empty;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetErrorLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        message = dr["Exception"].ToString();
                    }
                }
                cn.Close();
            }
            return message;

        }

        public static List<ErrorLog> List(string logLevel)
        {
            List<ErrorLog> list_el = new List<ErrorLog>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetErrorLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LogLevel", logLevel);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ErrorLog el = new ErrorLog();
                        Set(dr, ref el);
                        list_el.Add(el);
                    }
                }
                cn.Close();
            }
            return list_el;
        }

        private static void Set(SqlDataReader dr, ref ErrorLog el)
        {
            if (dr.HasRows)
            {
                el.Id = Convert.ToInt32(dr["Id"].ToString());
                el.LogDate = Convert.ToDateTime(dr["LogDate"].ToString());
                el.LogLevel = dr["LogLevel"].ToString();
                el.LogSource = dr["LogSource"].ToString();
                el.LogMessage = dr["LogMessage"].ToString();
                el.Exception = dr["Exception"].ToString();
            }
        }
    }
}