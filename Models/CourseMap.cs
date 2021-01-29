using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapping.Models
{
    public class CourseMap
    {
        #region Properties
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public bool Critical { get; set; }
        public bool CommonProgramPrerequiste { get; set; }
        public bool Required { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int Semester { get; set; }

        public string UCFCourseCode { get; set; }
        public string UCFCourseName { get; set; }
        public int UCFCredits { get; set; }
        public bool UCFCritical { get; set; }
        public bool UCFCommonProgramPrerequiste { get; set; }
        public bool UCFRequired { get; set; }
        public string UCFDescription { get; set; }
        public bool UCFActive { get; set; }
        public int UCFSemester { get; set; }
        #endregion

        public CourseMap()
        {

        }

        public CourseMap(Course c)
        {
            CourseCode = c.Code;
            CourseName = c.Name;
            Credits = c.Credits;
            Critical = c.Critical;
            CommonProgramPrerequiste = c.CommonProgramPrerequiste;
            Required = c.Required;
            Description = c.Description;
            Active = c.Active;
            Semester = c.Semester;
        }

        public static void SetUCFCourseMap(ref CourseMap cm, Course c)
        {
            cm.UCFCourseCode = c.Code;
            cm.UCFCourseName = c.Name;
            cm.UCFCredits = c.Credits;
            cm.UCFCritical = c.Critical;
            cm.UCFCommonProgramPrerequiste = c.CommonProgramPrerequiste;
            cm.UCFRequired = c.Required;
            cm.UCFDescription = c.Description;
            cm.UCFActive = c.Active;
            cm.UCFSemester = c.Semester;
        }

        public static List<CourseMap> List(int degreeId)
        {
            List<CourseMap> list_courseMap = new List<CourseMap>();
            List<Course> list_course = Course.List(degreeId);
            foreach (Course c in list_course)
            {
                CourseMap cm = new CourseMap(c);
                if (c.UCFCourseId.HasValue)
                {
                    Course UCFCourse = new Course();
                    UCFCourse = Course.Get(c.UCFCourseId.Value);
                    CourseMap.SetUCFCourseMap(ref cm, UCFCourse);
                }
                list_courseMap.Add(cm);
            }
            return list_courseMap.OrderBy(x=>x.UCFCourseCode).ToList();
        }

        public static List<CourseMap> UCFList(int UCFdegreeId)
        {
            List<CourseMap> list_courseMap = new List<CourseMap>();
            List<Course> list_c = DegreeMapping.Models.Course.List(UCFdegreeId);
            foreach (DegreeMapping.Models.Course c in list_c.Where(x=>x.Semester >= 5).OrderBy(x=>x.Semester).ThenBy(x=>x.Code))
            {
                CourseMap cm = new CourseMap();
                CourseMap.SetUCFCourseMap(ref cm, c);
                list_courseMap.Add(cm);
            }
            return list_courseMap;
        }
    }
}