using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc.Ajax;
using System.ComponentModel;

namespace DegreeMapping.Models
{
    public class Institution
    {
        public static int UCFId { get { return 1; } }
        public static int GenericId { get { return 7; } }
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public bool Active { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public string Description { get; set; }

        public Institution()
        {
            Active = true;
            NID = HttpContext.Current.User.Identity.Name;
        }

        public static int Insert(Institution i)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertInstitution";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", i.Name);
                cmd.Parameters.AddWithValue("@Url", i.URL);
                cmd.Parameters.AddWithValue("@Active", i.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", i.NID);
                cmd.Parameters.AddWithValue("@Description", i.Description);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static void Update(Institution i)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateInstitution";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", i.Id);
                cmd.Parameters.AddWithValue("@Name", i.Name);
                cmd.Parameters.AddWithValue("@Url", i.URL);
                cmd.Parameters.AddWithValue("@Active", i.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", i.NID);
                cmd.Parameters.AddWithValue("@Description", i.Description);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static List<Institution> List()
        {
            List<Institution> list_i = new List<Institution>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetInstitution";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Institution i = new Institution();
                        Set(dr, ref i);
                        list_i.Add(i);
                    }
                }
                cn.Close();
            }
            return list_i;
        }

        public static Institution Get(int id)
        {
            Institution i = new Institution();
            List<Institution> list_i = List();
            if (list_i.Exists(x => x.Id == id))
            {
                i = list_i.Where(x => x.Id == id).FirstOrDefault();
            }
            return i;
        }

        private static void Set(SqlDataReader dr, ref Institution i)
        {
            if (dr.HasRows)
            {
                i.Id = Convert.ToInt32(dr["Id"].ToString());
                i.Name = dr["Name"].ToString();
                i.URL = dr["URL"].ToString();
                i.Active = Convert.ToBoolean(dr["Active"].ToString());
                i.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                i.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                i.NID = dr["NID"].ToString();
                i.Description = dr["Description"].ToString();
            }
        }
    }
}