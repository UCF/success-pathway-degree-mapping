using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;

namespace DegreeMapping.Models
{
    public class User
    {
        public bool Authenticated { get; set; }
        public bool Authorized { get; set; }
        public string  DisplayName {get; set; }
        public string Email { get; set; }
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public User()
        { 
        }
        public User(string nid)
        {
            NID = nid;
            Allowed(nid);
        }

        public static void Insert(string nid)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", nid);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public void Allowed(string nid)
        {
            Authorized = false;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", nid);
                string userNID = cmd.ExecuteScalar().ToString();
                if (!string.IsNullOrEmpty(userNID))
                {
                    Authorized = true;
                }
                cn.Close();
            }
        }

        public static List<string> List()
        {
            List<string> list_users = new List<string>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetUser";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        list_users.Add(dr["NID"].ToString());
                    }
                }
                cn.Close();
            }
            return list_users;
        }

        public static void Delete(string nid)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "DeleteUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", nid);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }
    }
}