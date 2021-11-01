using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;

namespace DegreeMapping.Models
{
    public class SemesterCourse
    {
        public string SemesterTerm { get; set; }
        public string Course { get; set; }
        public int Credit { get; set; }

        public SemesterCourse()
        { 
        
        }

        public SemesterCourse(Course c)
        {
            SemesterTerm = c.SemesterTerm;
            Course = c.Code;
            Credit = c.Credits;
        }

        public static List<SemesterCourse> List(int degreeId)
        {
            List<SemesterCourse> list_semcourses = new List<SemesterCourse>();
            Degree d = Degree.Get(degreeId);
            if (d.UCFDegreeId.HasValue) {
                List<Course> list_c = DegreeMapping.Models.Course.List(d.UCFDegreeId.Value, null);
                foreach (Course c in list_c.Where(x=>x.Semester > 1).OrderBy(x=>x.Semester).ThenBy(x=>x.Term))
                {
                    list_semcourses.Add(new SemesterCourse(c));
                }
            }
            return list_semcourses;
        }
    }
}