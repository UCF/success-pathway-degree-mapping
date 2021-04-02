using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace DegreeMapping.Models
{
    public class CourseMapper
    {
        public struct Operand
        { 
            public static int EQUAL { get { return 0; } }
            public static int AND { get { return 1; } }
            public static int OR { get { return 2; } }
        }

        public int Id { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }


        public List<int> PartnerCourseIds { get; set; }
        public List<Course> PartnerCourses { get; set; }

        public int OperandType { get; set; }

        public List<int> UCFCourseIds { get; set; }
        public List<Course> UCFCourses { get; set; }

        public int OrderBy { get; set; }
        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public CourseMapper()
        {
            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();
            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();
            OrderBy = 1;
        }

        public CourseMapper(int degreeId)
        {
            DegreeId = degreeId;
            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();
            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();
            OrderBy = 1;
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
                cmd.Parameters.AddWithValue("@PartnerCourseIds", string.Join(",", cm.PartnerCourseIds));
                cmd.Parameters.AddWithValue("@UCFCourseIds", string.Join(",", cm.UCFCourseIds));
                cmd.Parameters.AddWithValue("@OperandType", cm.OperandType);
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
                cmd.Parameters.AddWithValue("@PartnerCourseIds", string.Join(",", cm.PartnerCourseIds));
                cmd.Parameters.AddWithValue("@UCFCourseIds", string.Join(",", cm.UCFCourseIds));
                cmd.Parameters.AddWithValue("@OperandType", cm.OperandType);
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
                cm.PartnerCourseIds = dr["PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                cm.UCFCourseIds = dr["UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                cm.Degree = dr["Degree"].ToString();
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                cm.Institution = dr["Institution"].ToString();
                cm.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                SetCourse(ref cm);
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
                    cm.PartnerCourses.Add(c);
                }
            }
        }
    }
}