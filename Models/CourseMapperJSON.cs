using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Migrations.Design;

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
            Course = c.Code;
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
            Course = c.Code;
            Credit = c.Credits;
        }
    }

    public class CourseMapperJSON
    {
        public List<UCFCourse> UCFCourses { get; set; }
        public List<PartnerCourse> PartnerCourses { get; set; }
        
        public int DisplayValue { get; set; }
        public List<UCFCourse> AlternateUCFCourse { get; set; }
        public List<PartnerCourse> AlternatePartnerCourse { get; set; }

        public CourseMapperJSON()
        {
            UCFCourses = new List<UCFCourse>();
            PartnerCourses = new List<PartnerCourse>();
            AlternateUCFCourse = new List<UCFCourse>();
            AlternatePartnerCourse = new List<PartnerCourse>();
            DisplayValue = 0;
        }

        public CourseMapperJSON(int degreeId)
        {
            UCFCourses = new List<UCFCourse>();
            PartnerCourses = new List<PartnerCourse>();
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
            foreach (CourseMapper cm in list_cm)
            {
                DisplayValue = cm.DisplayValue;
                foreach (Course c in cm.UCFCourses)
                {
                    UCFCourses.Add(new UCFCourse(c));
                }
                foreach (Course c in cm.PartnerCourses)
                {
                    PartnerCourses.Add(new PartnerCourse(c));
                }
                if (cm.AlternateUCFCourses.Count > 0)
                {
                    foreach (Course c in cm.AlternateUCFCourses)
                    {
                        AlternateUCFCourse.Add(new UCFCourse(c));
                    }
                }
                if (cm.AlternatePartnerCourses.Count > 0)
                {
                    foreach (Course c in cm.AlternatePartnerCourses)
                    {
                        AlternatePartnerCourse.Add(new PartnerCourse(c));
                    }
                }
            }
        }

        public static List<CourseMapperJSON> List(int degreeId)
        {
            List<CourseMapperJSON> list_cmj = new List<CourseMapperJSON>();
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);

            foreach (CourseMapper cm in list_cm)
            {
                CourseMapperJSON cmj = new CourseMapperJSON();
                foreach (Course c in cm.UCFCourses)
                {
                    cmj.UCFCourses.Add(new UCFCourse(c));
                }
                foreach (Course c in cm.PartnerCourses)
                {
                    cmj.PartnerCourses.Add(new PartnerCourse(c));
                }
                if(cm.AlternateUCFCourses.Count > 0)
                {
                    foreach (Course c in cm.AlternateUCFCourses)
                    {
                        cmj.AlternateUCFCourse.Add(new UCFCourse(c));
                    } 
                }
                if (cm.AlternatePartnerCourses.Count > 0)
                {
                    foreach (Course c in cm.AlternatePartnerCourses)
                    {
                        cmj.AlternatePartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                list_cmj.Add(cmj);
            }
            return list_cmj;
        }
    }
}