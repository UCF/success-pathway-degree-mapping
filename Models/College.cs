using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DegreeMapping.Models
{
    public class College
    {
        public int Id { get; set; }
        [DisplayName("College Name")]
        public string CollegeName { get; set; }
        public string Url { get; set; }

        public College()
        {

        }
        public static int Insert(College c)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertCollege";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CollegeName", c.CollegeName);
                cmd.Parameters.AddWithValue("@Url", c.Url);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static void Update(College c)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateCollege";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", c.Id);
                cmd.Parameters.AddWithValue("@CollegeName", c.CollegeName);
                cmd.Parameters.AddWithValue("@Url", c.Url);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static College Get(int id)
        {
            College c = List(id).FirstOrDefault();
            return c;
        }

        public static List<College> List(int? id)
        {
            List<College> list_c = new List<College>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCollege";
                cmd.CommandType = CommandType.StoredProcedure;
                if (id.HasValue)
                {
                    cmd.Parameters.AddWithValue("@Id", id.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        College c = new College();
                        set(dr, ref c);
                        list_c.Add(c);
                    }
                }
                cn.Close();
            }
            return list_c;
        }

        private static void set(SqlDataReader dr, ref College c)
        {
            if (dr.HasRows)
            {
                c.Id = Convert.ToInt32(dr["Id"].ToString());
                c.CollegeName = dr["College"].ToString();
                c.Url = dr["Url"].ToString();
            }
        }
    }
}