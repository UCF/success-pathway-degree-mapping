using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class Role
    {
        public static string SuperAdmin { get { return "Super Admin"; } }
        public static string Admin { get { return "Admin"; } }
        public static string Publisher { get { return "Publisher"; } }
        public static string Editor { get { return "Editor"; } }

        public Role()
        {

        }

        public static string Get(int id)
        {
            switch (id)
            {
                case 1: return Role.SuperAdmin;
                case 2: return Role.Admin;
                case 3: return Role.Publisher;
                default: return Role.Editor;
            }
        }

        public static Dictionary<int, string> List()
        {
            Dictionary<int, string> dict_Roles =new Dictionary<int, string>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetRole";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["Id"].ToString());
                        string role = dr["Role"].ToString();
                        dict_Roles.Add(id, role);
                    }
                }
                cn.Close();
            }
            return dict_Roles;
        }

        public static void Insert(string role)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertRole";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@role", role);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static void Update(int id, string role)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateRole";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }
    }
}