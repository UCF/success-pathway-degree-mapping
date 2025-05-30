﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;

namespace DegreeMapping.Models
{
    public class CourseMapper
    {
        public struct DisplayType
        {
            public static string Default { get { return "Default"; } }
            public static string Alternate { get { return "Alternate"; } }
            public static string SelectOne { get { return "Select One"; } }
            public static string SelectTwo { get { return "Select Two"; } }
            public static string SelectThree { get { return "Select Three"; } }
            public static string SelectFour { get { return "Select Four"; } }
            public static string OR { get { return "OR"; } }
            public static string AND { get { return "AND"; } }
        }

        public int Id { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }

        public int CatalogId { get; set; }
        public string CatalogYear { get; set; }

        public int SortOrder { get; set; }

        public List<int> UCFCourseIds { get; set; }
        public List<Course> UCFCourses { get; set; }

        public List<int> PartnerCourseIds { get; set; }
        public List<Course> PartnerCourses { get; set; }

        public List<int> AlternateUCFCourseIds { get; set; }
        public List<Course> AlternateUCFCourses { get; set; }

        public List<int> AlternatePartnerCourseIds { get; set; }
        public List<Course> AlternatePartnerCourses { get; set; }

        public string DisplayName { get; set; }
        public int DisplayValue { get; set; }

        public string AlternateDisplayName { get; set; }
        public int AlternateDisplayValue { get; set; }

        public List<int> Alternate2PartnerCourseIds { get; set; }
        public List<int> Alternate2UCFCourseIds { get; set; }
        public List<Course> Alternate2PartnerCourses { get; set; }
        public List<Course> Alternate2UCFCourses { get; set; }
        public string Alternate2DisplayName { get; set; }
        public int Alternate2DisplayValue { get; set; }

        public List<int> Alternate3PartnerCourseIds { get; set; }
        public List<int> Alternate3UCFCourseIds { get; set; }
        public List<Course> Alternate3PartnerCourses { get; set; }
        public List<Course> Alternate3UCFCourses { get; set; }
        public string Alternate3DisplayName { get; set; }
        public int Alternate3DisplayValue { get; set; }

        public List<int> Alternate4PartnerCourseIds { get; set; }
        public List<int> Alternate4UCFCourseIds { get; set; }
        public List<Course> Alternate4PartnerCourses { get; set; }
        public List<Course> Alternate4UCFCourses { get; set; }
        public string Alternate4DisplayName { get; set; }
        public int Alternate4DisplayValue { get; set; }

        public List<int> Alternate5PartnerCourseIds { get; set; }
        public List<int> Alternate5UCFCourseIds { get; set; }
        public List<Course> Alternate5PartnerCourses { get; set; }
        public List<Course> Alternate5UCFCourses { get; set; }
        public string Alternate5DisplayName { get; set; }
        public int Alternate5DisplayValue { get; set; }

        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public int? CloneCourseMapperId { get; set; }

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
            CatalogId = d.CatalogId;

            #region primary
            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();

            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();
            #endregion

            #region alternate
            AlternateDisplayValue = 0;
            AlternatePartnerCourseIds = new List<int>();
            AlternatePartnerCourses = new List<Course>();
            AlternateUCFCourseIds = new List<int>();
            AlternateUCFCourses = new List<Course>();
            #endregion

            #region alternate 2
            Alternate2DisplayValue = 0;
            Alternate2PartnerCourseIds = new List<int>();
            Alternate2PartnerCourses = new List<Course>();
            Alternate2UCFCourseIds = new List<int>();
            Alternate2UCFCourses = new List<Course>();
            #endregion

            #region alternate 3
            Alternate3DisplayValue = 0;
            Alternate3PartnerCourseIds = new List<int>();
            Alternate3PartnerCourses = new List<Course>();
            Alternate3UCFCourseIds = new List<int>();
            Alternate3UCFCourses = new List<Course>();
            #endregion

            #region alternate 4
            Alternate4DisplayValue = 0;
            Alternate4PartnerCourseIds = new List<int>();
            Alternate4PartnerCourses = new List<Course>();
            Alternate4UCFCourseIds = new List<int>();
            Alternate4UCFCourses = new List<Course>();
            #endregion

            #region alternate 5
            Alternate5DisplayValue = 0;
            Alternate5PartnerCourseIds = new List<int>();
            Alternate5PartnerCourses = new List<Course>();
            Alternate5UCFCourseIds = new List<int>();
            Alternate5UCFCourses = new List<Course>();
            #endregion

            DisplayValue = 0;
        }

        public static int Insert(CourseMapper cm)
        {
            List<CourseMapper> existingCM = CourseMapper.List(cm.DegreeId, null, null);
            if (existingCM != null && existingCM.Count > 0)
            {
                cm.SortOrder = existingCM.Max(x => x.SortOrder) + 1;
            }
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", cm.DegreeId);
                cmd.Parameters.AddWithValue("@SortOrder", cm.SortOrder);
                #region Primary
                cmd.Parameters.AddWithValue("@UCFCourseIds", string.Join(",", cm.UCFCourseIds));
                cmd.Parameters.AddWithValue("@PartnerCourseIds", string.Join(",", cm.PartnerCourseIds));
                cmd.Parameters.AddWithValue("@DisplayValue", string.Join(",", cm.DisplayValue));
                #endregion
                #region Alternate
                cmd.Parameters.AddWithValue("@AlternateUCFCourseIds", string.Join(",", cm.AlternateUCFCourseIds));
                cmd.Parameters.AddWithValue("@AlternatePartnerCourseIds", string.Join(",", cm.AlternatePartnerCourseIds));
                cmd.Parameters.AddWithValue("@AlternateDisplayValue", cm.AlternateDisplayValue);
                #endregion
                #region Alternate 2
                if (cm.Alternate2UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate2UCFCourseIds", string.Join(",", cm.Alternate2UCFCourseIds));
                }
                if (cm.Alternate2PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate2PartnerCourseIds", string.Join(",", cm.Alternate2PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate2DisplayValue", cm.Alternate2DisplayValue);
                #endregion
                #region Alternate 3
                if (cm.Alternate3UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate3UCFCourseIds", string.Join(",", cm.Alternate3UCFCourseIds));
                }
                if (cm.Alternate3PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate3PartnerCourseIds", string.Join(",", cm.Alternate3PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate3DisplayValue", cm.Alternate3DisplayValue);
                #endregion
                #region Alternate 4
                if (cm.Alternate4UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate4UCFCourseIds", string.Join(",", cm.Alternate4UCFCourseIds));
                }
                if (cm.Alternate4PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate4PartnerCourseIds", string.Join(",", cm.Alternate4PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate4DisplayValue", cm.Alternate4DisplayValue);
                #endregion
                #region Alternate 5
                if (cm.Alternate5UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate5UCFCourseIds", string.Join(",", cm.Alternate5UCFCourseIds));
                }
                if (cm.Alternate5PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate5PartnerCourseIds", string.Join(",", cm.Alternate5PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate5DisplayValue", cm.Alternate5DisplayValue);
                #endregion
                if (cm.CloneCourseMapperId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CloneCourseMapperId", cm.CloneCourseMapperId.Value);
                }
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
                cmd.Parameters.AddWithValue("@SortOrder", cm.SortOrder);
                #region Primary
                cmd.Parameters.AddWithValue("@UCFCourseIds", string.Join(",", cm.UCFCourseIds));
                cmd.Parameters.AddWithValue("@PartnerCourseIds", string.Join(",", cm.PartnerCourseIds));
                cmd.Parameters.AddWithValue("@DisplayValue", string.Join(",", cm.DisplayValue));
                #endregion
                #region Alternate
                cmd.Parameters.AddWithValue("@AlternateUCFCourseIds", string.Join(",", cm.AlternateUCFCourseIds));
                cmd.Parameters.AddWithValue("@AlternatePartnerCourseIds", string.Join(",", cm.AlternatePartnerCourseIds));
                cmd.Parameters.AddWithValue("@AlternateDisplayValue", cm.AlternateDisplayValue);
                #endregion
                #region Alternate 2
                if (cm.Alternate2UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate2UCFCourseIds", string.Join(",", cm.Alternate2UCFCourseIds));
                }
                if (cm.Alternate2PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate2PartnerCourseIds", string.Join(",", cm.Alternate2PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate2DisplayValue", cm.Alternate2DisplayValue);
                #endregion
                #region Alternate 3
                if (cm.Alternate3UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate3UCFCourseIds", string.Join(",", cm.Alternate3UCFCourseIds));
                }
                if (cm.Alternate3PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate3PartnerCourseIds", string.Join(",", cm.Alternate3PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate3DisplayValue", cm.Alternate3DisplayValue);
                #endregion
                #region Alternate 4
                if (cm.Alternate4UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate4UCFCourseIds", string.Join(",", cm.Alternate4UCFCourseIds));
                }
                if (cm.Alternate4PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate4PartnerCourseIds", string.Join(",", cm.Alternate4PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate4DisplayValue", cm.Alternate4DisplayValue);
                #endregion
                #region Alternate 5
                if (cm.Alternate5UCFCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate5UCFCourseIds", string.Join(",", cm.Alternate5UCFCourseIds));
                }
                if (cm.Alternate5PartnerCourseIds != null)
                {
                    cmd.Parameters.AddWithValue("@Alternate5PartnerCourseIds", string.Join(",", cm.Alternate5PartnerCourseIds));
                }
                cmd.Parameters.AddWithValue("@Alternate5DisplayValue", cm.Alternate5DisplayValue);
                #endregion
                if (cm.CloneCourseMapperId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CloneCourseMapperId", cm.CloneCourseMapperId.Value);
                }
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static void UpdateSortOrder(int courserMapperId, int sortOrderValue)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateCourseMapperOrderBy";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CouserMapperId", courserMapperId);
                cmd.Parameters.AddWithValue("@SortOrderValue", sortOrderValue);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static List<CourseMapper> List(int? degreeId, int? id, int? catalogId)
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
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
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
            CourseMapper cm = List(null, id, null).FirstOrDefault();
            return cm;
        }

        private static void SetCourseMapper(SqlDataReader dr, ref CourseMapper cm)
        {
            if (dr.HasRows)
            {
                cm.Id = Convert.ToInt32(dr["Id"].ToString());
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());

                #region Primary
                cm.DisplayValue = Convert.ToInt32(dr["DisplayValue"].ToString());
                cm.DisplayName = SetDisplayName(cm.DisplayValue);

                int sortOrder = 0;
                Int32.TryParse(dr["OrderBy"].ToString(), out sortOrder);
                cm.SortOrder = sortOrder;
                //cm.SortOrder = Convert.ToInt32(dr["OrderBy"].ToString());


                if (!string.IsNullOrEmpty(dr["PartnerCourseIds"].ToString()))
                {
                    cm.PartnerCourseIds = dr["PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.PartnerCourses = SetCourse(cm.PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["UCFCourseIds"].ToString()))
                {
                    cm.UCFCourseIds = dr["UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.UCFCourses = SetCourse(cm.UCFCourseIds);
                }
                #endregion
                #region Alternate
                cm.AlternateDisplayValue = Convert.ToInt32(dr["AlternateDisplayValue"].ToString());
                cm.AlternateDisplayName = SetDisplayName(cm.AlternateDisplayValue);
                if (!string.IsNullOrEmpty(dr["AlternatePartnerCourseIds"].ToString()))
                {
                    cm.AlternatePartnerCourseIds = dr["AlternatePartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.AlternatePartnerCourses = SetCourse(cm.AlternatePartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["AlternateUCFCourseIds"].ToString()))
                {
                    cm.AlternateUCFCourseIds = dr["AlternateUCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.AlternateUCFCourses = SetCourse(cm.AlternateUCFCourseIds);
                }
                #endregion
                #region Alternate 2
                cm.Alternate2DisplayValue = Convert.ToInt32(dr["Alternate2DisplayValue"].ToString());
                cm.Alternate2DisplayName = SetDisplayName(cm.Alternate2DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate2PartnerCourseIds"].ToString()))
                {
                    cm.Alternate2PartnerCourseIds = dr["Alternate2PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate2PartnerCourses = SetCourse(cm.Alternate2PartnerCourseIds);
                }
                if (!string.IsNullOrEmpty(dr["Alternate2UCFCourseIds"].ToString()))
                {
                    cm.Alternate2UCFCourseIds = dr["Alternate2UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate2UCFCourses = SetCourse(cm.Alternate2UCFCourseIds);
                }
                #endregion
                #region Alternate 3
                cm.Alternate3DisplayValue = Convert.ToInt32(dr["Alternate3DisplayValue"].ToString());
                cm.Alternate3DisplayName = SetDisplayName(cm.Alternate3DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate3PartnerCourseIds"].ToString()))
                {
                    cm.Alternate3PartnerCourseIds = dr["Alternate3PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate3PartnerCourses = SetCourse(cm.Alternate3PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["Alternate3UCFCourseIds"].ToString()))
                {
                    cm.Alternate3UCFCourseIds = dr["Alternate3UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate3UCFCourses = SetCourse(cm.Alternate3UCFCourseIds);
                }
                #endregion

                #region Alternate 4
                cm.Alternate4DisplayValue = Convert.ToInt32(dr["Alternate4DisplayValue"].ToString());
                cm.Alternate4DisplayName = SetDisplayName(cm.Alternate4DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate4PartnerCourseIds"].ToString()))
                {
                    cm.Alternate4PartnerCourseIds = dr["Alternate4PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate4PartnerCourses = SetCourse(cm.Alternate4PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["Alternate4UCFCourseIds"].ToString()))
                {
                    cm.Alternate4UCFCourseIds = dr["Alternate4UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate4UCFCourses = SetCourse(cm.Alternate4UCFCourseIds);
                }
                #endregion

                #region Alternate 5
                string alternate5DisplayValue = dr["Alternate5DisplayValue"].ToString();
                cm.Alternate5DisplayValue = (!string.IsNullOrEmpty(alternate5DisplayValue)) ? Convert.ToInt32(dr["Alternate5DisplayValue"].ToString()) : 0;
                cm.Alternate5DisplayName = SetDisplayName(cm.Alternate5DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate5PartnerCourseIds"].ToString()))
                {
                    cm.Alternate5PartnerCourseIds = dr["Alternate5PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate5PartnerCourses = SetCourse(cm.Alternate5PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["Alternate5UCFCourseIds"].ToString()))
                {
                    cm.Alternate5UCFCourseIds = dr["Alternate5UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate5UCFCourses = SetCourse(cm.Alternate5UCFCourseIds);
                }
                #endregion
                cm.Degree = dr["Degree"].ToString();
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                cm.Institution = dr["Institution"].ToString();
                cm.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                cm.CatalogYear = dr["CatalogYear"].ToString();
                cm.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                int cloneCourseMapperId;
                Int32.TryParse(dr["CloneCourseMapperId"].ToString(), out cloneCourseMapperId);
                cm.CloneCourseMapperId = cloneCourseMapperId;
                //cm.DisplayValue = Convert.ToInt32(dr["DisplayValue"].ToString());
                //SetDisplayName(ref cm);
                //SetCourse(ref cm);
            }
        }

        public static string SetDisplayName(int value)
        {
            switch (value)
            {
                case 7: return CourseMapper.DisplayType.SelectFour;
                case 6: return CourseMapper.DisplayType.SelectThree;
                case 5: return CourseMapper.DisplayType.SelectTwo;
                case 4: return CourseMapper.DisplayType.SelectOne;
                case 3: return CourseMapper.DisplayType.OR;
                case 2: return CourseMapper.DisplayType.AND;
                case 1: return CourseMapper.DisplayType.Alternate;
                default: return CourseMapper.DisplayType.Default;
            }
        }

        private static List<Course> SetCourse(List<int> courseIds)
        {
            List<Course> list_c = new List<Course>();
            foreach (int id in courseIds)
            {
                Course c = Course.Get(id);
                list_c.Add(c);
            }
            return list_c;
        }

        public static void Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "DeleteCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        //public static void SetSortOrderBy()
        //{
        //    int position = 100;
        //    List<Degree> list_d = DegreeMapping.Models.Degree.List(null, null);
        //    foreach (Degree d in list_d)
        //    {
        //        List<CourseMapper> list_cm = CourseMapper.List(d.Id, null, null);
        //        foreach (CourseMapper cm in list_cm.OrderBy(x=>x.SortOrder))
        //        {
        //            cm.SortOrder = position;
        //            position++;
        //            UpdateSortOrder(cm.Id, cm.SortOrder);
        //        }
        //        position = 100;
        //    }
        //}

        public static List<int> GetCourseMapperDistinctDegreeId()
        {
            List<int> list_degreeIds = new List<int>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCourseMapperDistinctDegreeId";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        list_degreeIds.Add(int.Parse(dr["DegreeId"].ToString()));
                    }
                }
                cn.Close();
            }
            return list_degreeIds;
        }

        public static void SwapSortOrder(int courseMapperId, int degreeId, string direction)
        {
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null, null);

            CourseMapper cmObj1 = list_cm.Where(x => x.Id == courseMapperId).FirstOrDefault();
            if (direction.ToLower() == "up")
            {

            }
            else
            {

            }


        }
    }
}