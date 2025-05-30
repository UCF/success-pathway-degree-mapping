﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;

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
        public string GPA { get; set; }
        [DisplayName("Limited Access")]
        public bool LimitedAccess { get; set; }
        [DisplayName("Restricted Access")]
        public bool RestrictedAccess { get; set; }
        public string Description { get; set; }
        [DisplayName("Catalog Year")]
        public string CatalogYear { get; set; }
        public int CatalogId { get; set; } 
        public bool Active { get; set; }
        public string Institution { get; set; }
        [DisplayName("Add Date")]
        public DateTime AddDate { get; set; }
        [DisplayName("Update Date")]
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public int? UCFDegreeId { get; set; }
        [DisplayName("UCF Degree Name")]
        public string UCFDegreeName { get; set; }
        [DisplayName("College Name")]
        public string CollegeName { get; set; }
        public int CollegeId { get; set; }
        [DisplayName("Degree URL")]
        public string DegreeURL { get; set; }
        [DisplayName("Catalog URL")]
        public string CatalogUrl { get; set; }
        public int? CloneDegreeId { get; set; }
        public int SemesterStart { get; set; }
        [DisplayName("Semester Start Term")]
        public string SemesterStartTerm { get; set; }
        [DisplayName("Career Path URL")]
        public string CareerPathURL { get; set; }
        [DisplayName("Global Course Notes")]
        public string GlobalCourseNotes { get; set; }

        [DisplayName("Display multiple semesters")]
        public bool DisplayMultipleSemesters { get; set; }

        [DisplayName("Degree is 100% Online")]
        public bool DegreeFullOnline { get; set; } = false;

        public Degree()
        {
            DisplayMultipleSemesters = true;
        }

        public Degree(int? catalogId)
        {
            Active = true;
            LimitedAccess = false;
            RestrictedAccess = false;
            NID = HttpContext.Current.User.Identity.Name;
            GPA = "2.0";
            UCFDegreeId = null;
            CatalogId = (catalogId.HasValue) ? catalogId.Value : new Catalog(true).Id;
            CatalogYear = (catalogId.HasValue) ? Catalog.Get(catalogId.Value).Year : new Catalog(true).Year;
            DisplayMultipleSemesters = true;
        }

        public Degree(int institutionId, int? catalogId)
        {
            Active = true;
            LimitedAccess = false;
            RestrictedAccess = false;
            InstitutionId = institutionId;
            Institution = DegreeMapping.Models.Institution.Get(institutionId).Name;
            NID = HttpContext.Current.User.Identity.Name;
            GPA = "2.0";
            UCFDegreeId = null;
            CatalogId = (catalogId.HasValue) ? catalogId.Value : new Catalog(true).Id;
            CatalogYear = (catalogId.HasValue) ? Catalog.Get(catalogId.Value).Year : new Catalog(true).Year;
            DisplayMultipleSemesters = true;
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
                cmd.Parameters.AddWithValue("@Name", d.Name.Trim());
                cmd.Parameters.AddWithValue("@GPA", d.GPA);
                cmd.Parameters.AddWithValue("@InstitutionId", d.InstitutionId);
                cmd.Parameters.AddWithValue("@DegreeType", d.DegreeType);
                cmd.Parameters.AddWithValue("@LimitedAccess", d.LimitedAccess);
                cmd.Parameters.AddWithValue("@RestrictedAccess", d.RestrictedAccess);
                cmd.Parameters.AddWithValue("@Description", d.Description);
                cmd.Parameters.AddWithValue("@CatalogId", d.CatalogId);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CollegeId", d.CollegeId);
                cmd.Parameters.AddWithValue("@DegreeUrl", d.DegreeURL);
                cmd.Parameters.AddWithValue("@CatalogUrl", d.CatalogUrl);
                cmd.Parameters.AddWithValue("@NID", d.NID);
                cmd.Parameters.AddWithValue("@SemesterStart", d.SemesterStart);
                cmd.Parameters.AddWithValue("@CareerPathURL", d.CareerPathURL);
                cmd.Parameters.AddWithValue("@DegreeFullOnline", d.DegreeFullOnline);
                if (d.UCFDegreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@UCFDegreeId", d.UCFDegreeId.Value);
                }
                //Only in the CloneUCFDegree method is the CloneDegreeId inserted, not on a new Degree
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
                cmd.Parameters.AddWithValue("@Name", d.Name.Trim());
                cmd.Parameters.AddWithValue("@GPA", d.GPA);
                cmd.Parameters.AddWithValue("@InstitutionId", d.InstitutionId);
                cmd.Parameters.AddWithValue("@DegreeType", d.DegreeType);
                cmd.Parameters.AddWithValue("@LimitedAccess", d.LimitedAccess);
                cmd.Parameters.AddWithValue("@RestrictedAccess", d.RestrictedAccess);
                cmd.Parameters.AddWithValue("@Description", d.Description);
                cmd.Parameters.AddWithValue("@CatalogId", d.CatalogId);
                cmd.Parameters.AddWithValue("@Active", d.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CollegeId", d.CollegeId);
                cmd.Parameters.AddWithValue("@DegreeUrl", d.DegreeURL);
                cmd.Parameters.AddWithValue("@CatalogUrl", d.CatalogUrl);
                cmd.Parameters.AddWithValue("@NID", d.NID);
                cmd.Parameters.AddWithValue("@SemesterStart", d.SemesterStart);
                cmd.Parameters.AddWithValue("@CareerPathURL", d.CareerPathURL);
                cmd.Parameters.AddWithValue("@DegreeFullOnline", d.DegreeFullOnline);
                if (d.CloneDegreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CloneDegreeId", d.CloneDegreeId.Value);
                }
                if (d.UCFDegreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@UCFDegreeId", d.UCFDegreeId.Value);
                }
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static List<Degree> List(int? instiutionId, int? catalogId)
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
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Degree d = new Degree(null);
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
            Degree d = new Degree(null);
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

        public static Degree GetClonedDegree(int cloneDegreeId)
        {
            Degree d = new Degree();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetClonedDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CloneDegreeId", cloneDegreeId);
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

        /// <summary>
        /// returns partner institutions only
        /// institutionId != 1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Degree> GetPartnerDegrees(int id)
        {
            List<Degree> list_degrees = new List<Degree>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetPartnerDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Degree d = new Degree();
                        Set(dr, ref d);
                        list_degrees.Add(d);
                    }
                }
                cn.Close();
            }
            return list_degrees;
        }

        private static void Set(SqlDataReader dr, ref Degree d)
        {
            if (dr.HasRows)
            {
                d.Id = Convert.ToInt32(dr["Id"].ToString());
                d.Name = dr["Name"].ToString();
                d.GPA = dr["GPA"].ToString();
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
                d.CollegeName = dr["CollegeName"].ToString();
                d.CollegeId = (!string.IsNullOrEmpty(d.CollegeName)) ? Convert.ToInt32(dr["CollegeId"].ToString()) : 0;
                d.DegreeURL = dr["DegreeUrl"].ToString();
                d.CatalogUrl = dr["CatalogUrl"].ToString();
                d.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                d.NID = dr["NID"].ToString();
                int ucfDegreeId;
                Int32.TryParse(dr["UCFDegreeId"].ToString(), out ucfDegreeId);
                d.UCFDegreeId = ucfDegreeId;
                d.UCFDegreeName = dr["UCFDegreeName"].ToString();
                d.CatalogYear = Catalog.Get(d.CatalogId).Year;
                int clonedegreeId;
                Int32.TryParse(dr["CloneDegreeId"].ToString(), out clonedegreeId);
                d.CloneDegreeId = clonedegreeId;
                d.SemesterStart = Convert.ToInt32(dr["SemesterStart"].ToString());
                d.CareerPathURL = dr["CareerPathURL"].ToString();
                d.GlobalCourseNotes = dr["GlobalCourseNotes"].ToString();
                d.DisplayMultipleSemesters = Convert.ToBoolean(dr["DisplayMultipleSemesters"].ToString());
                d.DegreeFullOnline = Convert.ToBoolean(dr["DegreeFullOnline"]);
                SetSemesterStartTerm(ref d);
            }
        }

        public static void UpdateGlobalCourseNotes(string globalCourseNotes, int degreeId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateGlobalCourseNotes";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", degreeId);
                cmd.Parameters.AddWithValue("@GlobalCourseNotes", globalCourseNotes.Trim());
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        private static void SetSemesterStartTerm(ref Degree d)
        {
            switch (d.SemesterStart)
            {
                case 1 : d.SemesterStartTerm = "Spring";
                    break;
                case 2: d.SemesterStartTerm = "Summer";
                    break;
                default: d.SemesterStartTerm = "Fall";
                    break;
            }
        }

        public static bool UpdateDisplayMultipleSemesters(bool displayMultipleSemesters, int degreeId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateDisplayMultipleSemesters";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", degreeId);
                cmd.Parameters.AddWithValue("@DisplayMultipleSemesters", displayMultipleSemesters);
                displayMultipleSemesters = Convert.ToBoolean(cmd.ExecuteScalar().ToString());
                cn.Close();
            }
            return displayMultipleSemesters;
        }
    }
}