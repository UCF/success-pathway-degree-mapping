using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;

namespace DegreeMapping.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int DegreeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public bool Active { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int InstitutionId { get; set; }


        public Note()
        {
            DegreeId = 0;
            Required = false;
            Active = true;
            NID = HttpContext.Current.User.Identity.Name;
        }

        public Note(int degreeId)
        {
            Degree d = DegreeMapping.Models.Degree.Get(degreeId);
            Active = true;
            this.Degree = d.Name;
            DegreeId = d.Id;
            this.Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            Required = true;
            Active = true;
            NID = HttpContext.Current.User.Identity.Name;
        }

        public static int Insert(Note n)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", n.DegreeId);
                cmd.Parameters.AddWithValue("@Name", n.Name);
                cmd.Parameters.AddWithValue("@Value", n.Value);
                cmd.Parameters.AddWithValue("@Required", n.Required);
                cmd.Parameters.AddWithValue("@Active", n.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", n.NID);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static void Update(Note n)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", n.Id);
                cmd.Parameters.AddWithValue("@DegreeId", n.DegreeId);
                cmd.Parameters.AddWithValue("@Name", n.Name);
                cmd.Parameters.AddWithValue("@Value", n.Value);
                cmd.Parameters.AddWithValue("@Required", n.Required);
                cmd.Parameters.AddWithValue("@Active", n.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", n.NID);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static List<Note> List(int degreeId)
        {
            List<Note> list_n = new List<Note>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", degreeId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Note n = new Note();
                        Set(dr, ref n);
                        list_n.Add(n);
                    }
                }
                cn.Close();
            }
            return list_n;
        }

        public static Note Get(int id)
        {
            Note n = new Note();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref n);
                    }
                }
                cn.Close();
            }
            return n;
        }

        private static void Set(SqlDataReader dr, ref Note n)
        {
            if (dr.HasRows)
            {
                n.Id = Convert.ToInt32(dr["Id"].ToString());
                n.Name = dr["Name"].ToString();
                n.Value = dr["Value"].ToString();
                n.Required = Convert.ToBoolean(dr["Required"].ToString());
                n.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                n.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                n.NID = dr["NID"].ToString();
                n.Active = Convert.ToBoolean(dr["Active"].ToString());
                n.Degree = dr["Degree"].ToString();
                n.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                n.Institution = dr["Institution"].ToString();
                n.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "DeleteNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }
    }
}