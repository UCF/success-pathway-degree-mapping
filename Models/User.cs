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
        public int RoleId { get; set; }

        public User()
        {
            RoleId = 3;
        }
        public User(string nid)
        {
            NID = nid;
            RoleId = 3;
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
                cmd.Parameters.AddWithValue("@RoleId", 3);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static void Update(string nid, int roleId, string displayName)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", nid);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@DisplayName", displayName);
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
                SqlDataReader dr = cmd.ExecuteReader();
                //string userNID = cmd.ExecuteScalar().ToString();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (!string.IsNullOrEmpty(dr["NID"].ToString()))
                        {
                            RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                            Authorized = true;
                            DisplayName = dr["DisplayName"].ToString();
                        }
                    }
                }
                cn.Close();
            }
        }


        public static User Get(string nid)
        {
            User user = new User();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", nid);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user.NID = (dr["NID"].ToString());
                        user.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                        user.DisplayName = dr["DisplayName"].ToString();
                    }
                }
                cn.Close();
            }
            return user;
        }

        public static List<User> List()
        {
            List<User> list_users = new List<User>();
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
                        User user = new User();
                        user.NID = (dr["NID"].ToString());
                        user.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                        user.DisplayName = dr["DisplayName"].ToString();
                        list_users.Add(user);
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