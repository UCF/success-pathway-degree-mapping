using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapping.Models
{
    public class UCFCourse
    {
        public string Course { get; set; }
        public int Credit { get; set; }
        public bool Required { get; set; }
        public bool CPP { get; set; }
        public bool Critical { get; set; }

        public UCFCourse()
        { 
        
        }

        public UCFCourse(Course c)
        {
            Course = c.Name;
            Credit = c.Credits;
            Required = c.Required;
            CPP = c.CommonProgramPrerequiste;
            Critical = c.Critical;
        }
    }

    public class PartnerCourse
    {
        public string Course { get; set; }
        public int Credit { get; set; }

        public PartnerCourse()
        { 
        
        }

        public PartnerCourse(Course c)
        {
            Course = c.Name;
            Credit = c.Credits;
        }

        public PartnerCourse(string course, int credit)
        {
            Course = course;
            Credit = credit;
        }
    }

    public class CourseMapperJSON
    {
        public List<UCFCourse> UCFCourses { get; set; }
        public List<PartnerCourse> ParterCourses { get; set; }

        public CourseMapperJSON()
        {
            UCFCourses = new List<UCFCourse>();
            ParterCourses = new List<PartnerCourse>();
        }

        /*
            THINK ABOUT HOW TO ORGANIZE THE DATA
        */
        public CourseMapperJSON(int degreeId)
        {
            UCFCourses = new List<UCFCourse>();
            ParterCourses = new List<PartnerCourse>();
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
            foreach (CourseMapper cm in list_cm)
            {
                foreach (Course c in cm.UCFCourses)
                {
                    UCFCourse ucfCourse = new UCFCourse(c);
                    PartnerCourse pCourse = new PartnerCourse(c);
                    UCFCourses.Add(ucfCourse);
                    ParterCourses.Add(pCourse);
                }
            }
        }

        

    }
}