using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;

namespace DegreeMapping.Models
{
    public class CourseMapper
    {
        public struct DisplayType 
        { 
            public static string Default { get { return "Default";  } }
            public static string SelectOne { get { return "Select One"; } }
        }

        public int Id { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }

        public int CatalogyId { get; set; }
        public string CatalogYear { get; set; }

        public List<int> UCFCourseIds { get; set; }
        public List<Course> UCFCourses { get; set; }

        public List<int> PartnerCourseIds { get; set; }
        public List<Course> PartnerCourses { get; set; }

        public List<int> AlternateUCFCourseIds { get; set; }
        public List<Course> AlternateUCFCourses { get; set; }

        public List<int> AlternatePartnerCourseIds { get; set; }
        public List<Course> AlternatePartnerCourses { get; set; }

        public int DisplayName { get; set; }
        public int DisplayValue { get; set; }



        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public CourseMapper()
        {
            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();

            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();

            AlternatePartnerCourseIds = new List<int>();
            AlternatePartnerCourses = new List<Course>();

            AlternateUCFCourseIds = new List<int>();
            AlternateUCFCourses = new List<Course>();

            DisplayValue = 0;
        }

        public CourseMapper(int degreeId)
        {
            Degree d = DegreeMapping.Models.Degree.Get(degreeId);
            DegreeId = d.Id;
            Degree = d.Name;
            Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            CatalogYear = d.CatalogYear;
            CatalogyId = d.CatalogId;

            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();

            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();

            AlternatePartnerCourseIds = new List<int>();
            AlternatePartnerCourses = new List<Course>();

            AlternateUCFCourseIds = new List<int>();
            AlternateUCFCourses = new List<Course>();

            DisplayValue = 0;
        }

        public static int Insert(CourseMapper cm)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", cm.DegreeId);
                cmd.Parameters.AddWithValue("@UCFCourseIds", string.Join(",", cm.UCFCourseIds));
                cmd.Parameters.AddWithValue("@PartnerCourseIds", string.Join(",", cm.PartnerCourseIds));
                cmd.Parameters.AddWithValue("@AlternateUCFCourseIds", string.Join(",", cm.AlternateUCFCourseIds));
                cmd.Parameters.AddWithValue("@AlternatePartnerCourseIds", string.Join(",", cm.AlternatePartnerCourseIds));
                cmd.Parameters.AddWithValue("@DisplayValue", string.Join(",", cm.DisplayValue));
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static void Update(CourseMapper cm)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cm.Id);
                cmd.Parameters.AddWithValue("@DegreeId", cm.DegreeId);
                cmd.Parameters.AddWithValue("@UCFCourseIds", string.Join(",", cm.UCFCourseIds));
                cmd.Parameters.AddWithValue("@PartnerCourseIds", string.Join(",", cm.PartnerCourseIds));
                cmd.Parameters.AddWithValue("@AlternateUCFCourseIds", string.Join(",", cm.AlternateUCFCourseIds));
                cmd.Parameters.AddWithValue("@AlternatePartnerCourseIds", string.Join(",", cm.AlternatePartnerCourseIds));
                cmd.Parameters.AddWithValue("@DisplayValue", string.Join(",", cm.DisplayValue));
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static List<CourseMapper> List(int? degreeId, int? id)
        {
            List<CourseMapper> list_cm = new List<CourseMapper>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                if (degreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@DegreeId", degreeId.Value);
                }
                if (id.HasValue)
                {
                    cmd.Parameters.AddWithValue("@Id", id.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CourseMapper cm = new CourseMapper();
                        SetCourseMapper(dr, ref cm);
                        list_cm.Add(cm);
                    }
                }
                cn.Close();
            }
            return list_cm;
        }

        public static CourseMapper Get(int id)
        {
            CourseMapper cm = List(null, id).FirstOrDefault();
            return cm;
        }

        private static void SetCourseMapper(SqlDataReader dr, ref CourseMapper cm)
        {
            if (dr.HasRows)
            {
                cm.Id = Convert.ToInt32(dr["Id"].ToString());
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());

                if (!string.IsNullOrEmpty(dr["PartnerCourseIds"].ToString()))
                {
                    cm.PartnerCourseIds = dr["PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                }

                if (!string.IsNullOrEmpty(dr["UCFCourseIds"].ToString()))
                {
                    cm.UCFCourseIds = dr["UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                }

                if (!string.IsNullOrEmpty(dr["AlternatePartnerCourseIds"].ToString()))
                {
                    cm.AlternatePartnerCourseIds = dr["AlternatePartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                }

                if (!string.IsNullOrEmpty(dr["AlternateUCFCourseIds"].ToString()))
                {
                    cm.AlternateUCFCourseIds = dr["AlternateUCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                }

                cm.Degree = dr["Degree"].ToString();
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                cm.Institution = dr["Institution"].ToString();
                cm.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                cm.CatalogYear = dr["CatalogYear"].ToString();
                cm.CatalogyId = Convert.ToInt32(dr["CatalogId"].ToString());
                cm.DisplayValue = Convert.ToInt32(dr["DisplayValue"].ToString());
                SetDisplayName(ref cm);
                SetCourse(ref cm);
            }
        }

        private static string SetDisplayName(ref CourseMapper cm)
        {
            if (cm.DisplayValue == 1) {
                return CourseMapper.DisplayType.SelectOne;
            }
            else 
            {
                return CourseMapper.DisplayType.Default;
            }
        }

        private static void SetCourse(ref CourseMapper cm)
        {
            if (cm.PartnerCourseIds.Count > 0) {
                foreach (int id in cm.PartnerCourseIds)
                {
                    Course c = Course.Get(id);
                    cm.PartnerCourses.Add(c);
                }
            }
            if (cm.UCFCourseIds.Count > 0)
            {
                foreach (int id in cm.UCFCourseIds)
                {
                    Course c = Course.Get(id);
                    cm.UCFCourses.Add(c);
                }
            }

            if (cm.AlternatePartnerCourseIds.Count > 0)
            {
                foreach (int id in cm.AlternatePartnerCourseIds)
                {
                    Course c = Course.Get(id);
                    cm.AlternatePartnerCourses.Add(c);
                }
            }

            if (cm.AlternateUCFCourseIds.Count > 0)
            {
                foreach (int id in cm.AlternateUCFCourseIds)
                {
                    Course c = Course.Get(id);
                    cm.AlternateUCFCourses.Add(c);
                }
            }

        }

        public static void Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "DeleteCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id",id);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}