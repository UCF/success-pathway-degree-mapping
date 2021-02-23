using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace DegreeMapping.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DisplayName("Issue")]
        public string IssueDesc { get; set; }
        public bool Resolved { get; set; }
        public string UpdatedBy {get; set; }
        public DateTime UpdatedDate { get; set; }

        public Issue()
        { 
        
        }

        public static int Insert(Issue i)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertIssue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", i.Title);
                cmd.Parameters.AddWithValue("@Issue", i.IssueDesc);
                cmd.Parameters.AddWithValue("@Resolved", i.Resolved);
                cmd.Parameters.AddWithValue("@UpdatedBy", i.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedDate", i.UpdatedDate);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }
        public static void Update(Issue i)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertIssue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", i.Id);
                cmd.Parameters.AddWithValue("@Title", i.Title);
                cmd.Parameters.AddWithValue("@Issue", i.IssueDesc);
                cmd.Parameters.AddWithValue("@Resolved", i.Resolved);
                cmd.Parameters.AddWithValue("@UpdatedBy", i.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedDate", i.UpdatedDate);
                cn.Close();
            }
        }
        public static Issue Get(int id)
        {
            Issue i = new Issue();
            i = Issue.List(id).FirstOrDefault();
            return i;
        }
        public static List<Issue> List(int? id)
        {
            List<Issue> list_i = new List<Issue>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "";
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
                        Issue i = new Issue();
                        Set(dr, ref i);
                    }
                }
                cn.Close();
            }
            return list_i;
        }
        private static void Set(SqlDataReader dr, ref Issue i)
        {
            if (dr.HasRows)
            {
                i.Id = Convert.ToInt32(dr["Id"].ToString());
                i.Title = dr["Title"].ToString();
                i.IssueDesc = dr["Issue"].ToString();
                i.Resolved = Convert.ToBoolean(dr["Resolved"].ToString());
                i.UpdatedBy = dr["UpdatedBy"].ToString();
                i.UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"].ToString());
            }
        }
    }
}