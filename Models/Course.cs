﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Globalization;

namespace DegreeMapping.Models
{
    public class Course
    {
        public struct CourseTerm
        {
            public static string Fall { get { return "Fall"; } }
            public static string Spring { get { return "Spring"; } }
            public static string Summer { get { return "Summer"; } }
        }

        public int Id { get; set; }
        [DisplayName("Degree Id")] 
        public int DegreeId { get; set; }
        [DisplayName("Course Name")]
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string CreditText { get; set; }
        public bool Critical { get; set; }
        [DisplayName("Common Program Prerequisite")]
        public bool CommonProgramPrerequiste { get; set; }
        public bool Required { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        [DisplayName("Institution Id")]
        public int InstitutionId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public int Semester { get; set; }
        public string Term { get; set; }
        public string SemesterTerm { get { return (!string.IsNullOrEmpty(Term)) ? Semester.ToString() + " " + Term: Semester.ToString(); } }
        public int? UCFCourseId { get; set; }
        [DisplayName("UCF Related Course")]
        public string UCFRelatedCourse { get; set; }
        public int UCFCourseCredits { get; set; }
        public int CatalogId { get; set; }
        [DisplayName("Catalog Year")]
        public string CatalogYear { get; set; }
        public int? CloneCourseId { get; set; }
        public string TermOrder { get; set; }

        public Course()
        {
            CommonProgramPrerequiste = true;
            Critical = true;
            Required = true;
            Active = true;
            Credits = 3;
            CreditText = string.Empty;
            NID = HttpContext.Current.User.Identity.Name;
            Semester = 1;
        }
        public Course(int degreeId)
        {
            CommonProgramPrerequiste = true;
            Required = true;
            Active = true;
            Credits = 3;
            CreditText = string.Empty;
            Critical = true;
            DegreeId = degreeId;
            DegreeMapping.Models.Degree d = DegreeMapping.Models.Degree.Get(degreeId);
            Degree = d.Name;
            DegreeId = d.Id;
            Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            CatalogYear = d.CatalogYear;
            CatalogId = d.CatalogId;
            NID = HttpContext.Current.User.Identity.Name;
            Semester = 1;
        }

        public static int Insert(Course c)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", c.DegreeId);
                cmd.Parameters.AddWithValue("@Code", c.Code.Trim());
                cmd.Parameters.AddWithValue("@Credits", c.Credits);
                cmd.Parameters.AddWithValue("@CreditText", c.CreditText);
                cmd.Parameters.AddWithValue("@Critical", c.Critical);
                cmd.Parameters.AddWithValue("@CommonProgramPrerequiste", c.CommonProgramPrerequiste);
                cmd.Parameters.AddWithValue("@Required", c.Required);
                cmd.Parameters.AddWithValue("@Description", c.Description);
                cmd.Parameters.AddWithValue("@Active", c.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", c.NID);
                cmd.Parameters.AddWithValue("@Semester", c.Semester);
                cmd.Parameters.AddWithValue("@Term", c.Term);
                if (c.UCFCourseId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@UCFCourseId", c.UCFCourseId);
                }
                if (c.CloneCourseId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CloneCourseId", c.CloneCourseId.Value);
                }
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static void Update(Course c)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", c.Id);
                cmd.Parameters.AddWithValue("@DegreeId", c.DegreeId);
                cmd.Parameters.AddWithValue("@Code", c.Code.Trim());
                cmd.Parameters.AddWithValue("@Credits", c.Credits);
                cmd.Parameters.AddWithValue("@CreditText", c.CreditText);
                cmd.Parameters.AddWithValue("@Critical", c.Critical);
                cmd.Parameters.AddWithValue("@CommonProgramPrerequiste", c.CommonProgramPrerequiste);
                cmd.Parameters.AddWithValue("@Required", c.Required);
                cmd.Parameters.AddWithValue("@Description", c.Description);
                cmd.Parameters.AddWithValue("@Active", c.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", c.NID);
                cmd.Parameters.AddWithValue("@Semester", c.Semester);
                cmd.Parameters.AddWithValue("@Term", c.Term);
                if (c.UCFCourseId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@UCFCourseId", c.UCFCourseId);
                }
                if (c.CloneCourseId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CloneCourseId", c.CloneCourseId.Value);
                }
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static List<Course> List(int? degreeId, int? catalogId)
        {
            List<Course> list_c = new List<Course>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                if (degreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@DegreeId", degreeId.Value);
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
                        Course c = new Course();
                        Set(dr, ref c);
                        list_c.Add(c);
                    }
                }
                cn.Close();
            }
            return list_c;
        }

        public static Course Get(int id)
        {
            Course c = new Course();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref c);
                    }
                }
                cn.Close();
            }
            return c;
        }

        public static Course GetClonedCourse(int clonedCourseId)
        {
            Course c = new Course();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetClonedCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClonedCourseId", clonedCourseId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref c);
                    }
                }
                cn.Close();
            }
            return c;
        }

        public static List<Course> Search(string keyword, int? catalogId)
        {
            List<Course> list_c = new List<Course>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "SearchCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@keyword", keyword);
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Course c = new Course();
                        Set(dr, ref c);
                        list_c.Add(c);
                    }
                }
                cn.Close();
            }
            return list_c;
        }

        private static void Set(SqlDataReader dr, ref Course c)
        {
            if (dr.HasRows)
            {
                c.Id = Convert.ToInt32(dr["Id"].ToString());
                c.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString().ToUpper());
                c.Code = dr["Code"].ToString();
                c.Credits = Convert.ToInt32(dr["Credits"].ToString());
                c.CreditText = dr["CreditText"].ToString();
                c.Critical = Convert.ToBoolean(dr["Critical"].ToString());
                c.CommonProgramPrerequiste = Convert.ToBoolean(dr["CommonProgramPrerequiste"].ToString());
                c.Required = Convert.ToBoolean(dr["Required"].ToString());
                c.Description = dr["Description"].ToString();
                c.Active = Convert.ToBoolean(dr["Active"].ToString());
                c.Degree = dr["Degree"].ToString();
                c.Institution = dr["Institution"].ToString();
                c.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                c.Active = Convert.ToBoolean(dr["Active"].ToString());
                c.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                c.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                c.NID = dr["NID"].ToString();
                c.Semester = Convert.ToInt32(dr["Semester"].ToString());
                c.Term = dr["Term"].ToString();
                c.CatalogYear = dr["CatalogYear"].ToString();
                c.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                int ucfCourseId;
                int ucfCourseCredits;
                c.UCFRelatedCourse = dr["UCFRelatedCourse"].ToString();
                Int32.TryParse(dr["UCFCourseId"].ToString(), out ucfCourseId);
                if (ucfCourseId > 0)
                {
                    c.UCFCourseId = ucfCourseId;
                }
                Int32.TryParse(dr["UCFCourseCredits"].ToString(), out ucfCourseCredits);
                if(ucfCourseCredits > 0)
                {
                    c.UCFCourseCredits = Convert.ToInt32(dr["UCFCourseCredits"].ToString());
                }
                int cloneCourseId;
                Int32.TryParse(dr["CloneCourseId"].ToString(), out cloneCourseId);
                c.TermOrder = dr["TermOrder"].ToString();
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "DeleteCourse";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }
    }
}