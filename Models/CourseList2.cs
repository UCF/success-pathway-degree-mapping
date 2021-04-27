using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class CourseList2
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

        public CourseList2()
        { 
        
        }
    }
}