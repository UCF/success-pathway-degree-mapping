using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DegreeMapping.Models
{
    public class Degree
    {
        public int Id { get; set; }
        [DisplayName("Institution Id")]
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        [DisplayName("Degree Type")]
        public string DegreeType { get; set; }
        [DisplayName("Limited Access")]
        public bool LimitedAccess {get;set;}
        [DisplayName("Restricted Access")]
        public bool RestrictedAccess { get; set; }
        public string Description { get; set; }

        [DisplayName("Catalog Year")]
        public string CatalogYear { get; set; }
        public bool Active { get; set; }

        public string Institution { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }

        public Degree()
        {
            Active = true;
            LimitedAccess = false;
            RestrictedAccess = false;
            NID = HttpContext.Current.User.Identity.Name;
        }

        public Degree(int institutionId)
        {
            Active = true;
            LimitedAccess = false;
            RestrictedAccess = false;
            InstitutionId = institutionId;
            Institution = DegreeMapping.Models.Institution.Get(institutionId).Name;
            NID = HttpContext.Current.User.Identity.Name;
        }

        public static int Insert(Degree d)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", d.Name);
                cmd.Parameters.AddWithValue("@InstitutionId", d.InstitutionId);
                cmd.Parameters.AddWithValue("@DegreeType", d.DegreeType);
                cmd.Parameters.AddWithValue("@LimitedAccess", d.LimitedAccess);
                cmd.Parameters.AddWithValue("@RestrictedAccess", d.RestrictedAccess);
                cmd.Parameters.AddWithValue("@Description", d.Description);
                cmd.Parameters.AddWithValue("@CatalogYear", d.CatalogYear);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", d.NID);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static int Update(Degree d)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", d.Id);
                cmd.Parameters.AddWithValue("@Name", d.Name);
                cmd.Parameters.AddWithValue("@InstitutionId", d.InstitutionId);
                cmd.Parameters.AddWithValue("@DegreeType", d.DegreeType);
                cmd.Parameters.AddWithValue("@LimitedAccess", d.LimitedAccess);
                cmd.Parameters.AddWithValue("@RestrictedAccess", d.RestrictedAccess);
                cmd.Parameters.AddWithValue("@Description", d.Description);
                cmd.Parameters.AddWithValue("@CatalogYear", d.CatalogYear);
                cmd.Parameters.AddWithValue("@Active", d.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", d.NID);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static List<Degree> List(int? instiutionId)
        {
            List<Degree> list_d = new List<Degree>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                if (instiutionId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@InstitutionId", instiutionId.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Degree d = new Degree();
                        Set(dr, ref d);
                        list_d.Add(d);
                    }
                }
                cn.Close();
            }
            return list_d;
        }

        public static Degree Get(int id)
        {
            Degree d = new Degree();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref d);
                    }
                }
                cn.Close();
            }
            return d;

        }

        private static void Set(SqlDataReader dr, ref Degree d)
        {
            if (dr.HasRows)
            {
                d.Id = Convert.ToInt32(dr["Id"].ToString());
                d.Name = dr["Name"].ToString();
                d.DegreeType = dr["DegreeType"].ToString();
                d.LimitedAccess = Convert.ToBoolean(dr["LimitedAccess"].ToString());
                d.RestrictedAccess = Convert.ToBoolean(dr["RestrictedAccess"].ToString());
                d.Description = dr["Description"].ToString();
                d.CatalogYear = dr["CatalogYear"].ToString();
                d.Institution = dr["Institution"].ToString();
                d.Active = Convert.ToBoolean(dr["Active"].ToString());
                d.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                d.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                d.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                d.NID = dr["NID"].ToString();
            }
        }
    }
}