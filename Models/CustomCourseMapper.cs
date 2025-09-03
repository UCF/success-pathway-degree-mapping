using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapping.Models
{
    public class CustomCourseMapper
    {
        public int DegreeId { get; set; }
        public List<int> List_CourseIds { get; set; }
        public List<Course> List_Courses { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int InstitutionId { get; set; }
        public string CatalogYear { get; set; }
        public int CatalogId { get; set; }
        public bool HasRecord { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string NID { get; set; } = "system";

        public CustomCourseMapper(int degreeId)
        {
            List_CourseIds = new List<int>();
            List_Courses = new List<Course>();
            Degree d = DegreeMapping.Models.Degree.Get(degreeId);
            DegreeId = d.Id;
            Degree = d.Name;
            Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            CatalogYear = d.CatalogYear;
            CatalogId = d.CatalogId;
            HasRecord = false;
        }

        public static void Insert(CustomCourseMapper ccm)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertCustomCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", ccm.NID);

                cmd.Parameters.AddWithValue("@DegreeId", ccm.DegreeId);
                if (ccm.List_CourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@CourseIds", string.Join(",", ccm.List_CourseIds).TrimEnd(','));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CourseIds", null);
                }
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void Update(CustomCourseMapper ccm)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateCustomCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", ccm.DegreeId);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", ccm.NID);
                if (ccm.List_CourseIds is null)
                {
                    cmd.Parameters.AddWithValue("@CourseIds", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CourseIds", string.Join(",", ccm.List_CourseIds).TrimEnd(','));
                }
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static CustomCourseMapper Get(int degreeId)
        {
            CustomCourseMapper ccm = new CustomCourseMapper(degreeId);
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCustomCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@degreeId", degreeId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read()) 
                    {
                        ccm.HasRecord = true;
                        if (!string.IsNullOrEmpty(dr["UpdatedDate"].ToString()))
                        {
                            ccm.UpdatedDate = null;
                        }

                        if (!string.IsNullOrEmpty(dr["NID"].ToString()))
                        {
                            ccm.NID = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(dr["CourseIds"].ToString()))
                        {
                            ccm.List_CourseIds = dr["CourseIds"].ToString().TrimEnd(',').Split(',').Select(Int32.Parse).ToList();
                        }
                        else
                        {
                            ccm.List_CourseIds = new List<int>();
                        }
                        
                        if (ccm.List_CourseIds.Count > 0)
                        {
                            
                            foreach (int id in ccm.List_CourseIds)
                            {
                                Course c = Course.Get(id);
                                ccm.List_Courses.Add(c);
                            }
                        }
                    }
                }
                cn.Close();
            }
            if (ccm.List_Courses.Count == 0)
            {
                //ccm.HasRecord = false;
                GetUCFDefaultSemesterCourses(ref ccm);
            }
            return ccm;
        }


        public static List<CustomCourseMapper> List(int catalogId)
        {
            List<CustomCourseMapper> list_ccm = new List<CustomCourseMapper>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCustomCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CataglogId", catalogId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int degreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                        CustomCourseMapper ccm = new CustomCourseMapper(degreeId);
                        GetUCFDefaultSemesterCourses(ref ccm);
                        list_ccm.Add(ccm);
                    }
                }
                cn.Close();
            }
            return list_ccm;
        }


        //gets UCF Default Courses
        public static void GetUCFDefaultSemesterCourses(ref CustomCourseMapper ccm)
        {
            Degree d = DegreeMapping.Models.Degree.Get(ccm.DegreeId);
            List<Course> list_ucfCourses = DegreeMapping.Models.Course.List(d.UCFDegreeId, null);
            ccm.List_CourseIds = list_ucfCourses.Where(x => x.Semester > 1).Select(x => x.Id).ToList();
            ccm.List_Courses = list_ucfCourses.Where(x => x.Semester > 1).ToList();
        }
    }
}