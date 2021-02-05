using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class CourseList
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public bool Critical { get; set; }
        public bool CommonProgramPrerequiste { get; set; }
        public bool Required { get; set; }
        public int Semester { get; set; }
        public int UCFCourseId { get; set; }
        public string UCFCourseName { get; set; }
        public int UCFCourseCredits { get; set; }


        public CourseList()
        { 
        
        }
        public CourseList(Course c)
        {
            Id = c.Id;
            CourseCode = c.Code;
            CourseName = c.Name;
            Credits = c.Credits;
            Critical = c.Critical;
            CommonProgramPrerequiste = c.CommonProgramPrerequiste;
            Required = c.Required;
            Semester = c.Semester;
            if (c.UCFCourseId.HasValue)
            {
                UCFCourseId = c.UCFCourseId.Value;
                UCFCourseName = c.UCFRelatedCourse;
                UCFCourseCredits = c.UCFCourseCredits;
            }
        }

        public static List<CourseList> List(int degreeId)
        {
            List<Course> list_c = Course.List(degreeId);
            List<CourseList> list_cl = new List<CourseList>();
            foreach (Course c in list_c.Where(x=>x.Active).OrderBy(x=>x.Code))
            {
                CourseList cl = new CourseList(c);
                list_cl.Add(cl);
            }
            return list_cl;
        }
    }
}