using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class DegreeMap
    {
        #region Properties
        public string Institution { get; set; }
        public string GPA { get; set; }
        public string Degree { get; set; }
        public bool Active { get; set; }
        public bool LimitedAccess { get; set; }
        public bool RestrictedAccess { get; set; }
        public string Description { get; set; }

        public string UCFInstitution { get; set; }
        public string UCFGPA { get; set; }
        public string UCFDegree { get; set; }
        public bool UCFActive { get; set; }
        public bool UCFLimitedAccess { get; set; }
        public bool UCFRestrictedAccess { get; set; }
        public string UCFDescription { get; set; }

        public List<CourseMap> CourseMap {get;set;}
        public List<CourseMap> UCFCourses { get; set; }

        public List<Note> DegreeNotes { get; set; }
        #endregion


        public DegreeMap()
        {
            CourseMap = new List<CourseMap>();
            DegreeNotes = new List<Note>();
            UCFCourses = new List<CourseMap>();
        }

        public DegreeMap(int InstitutionId, int degreeId)
        {
            DegreeMapping.Models.Degree d = DegreeMapping.Models.Degree.Get(degreeId);
            Institution = d.Institution;
            GPA = d.GPA;
            this.Degree = d.Name;
            Active = d.Active;
            LimitedAccess = d.LimitedAccess;
            RestrictedAccess = d.RestrictedAccess;
            Description = d.Description;
            if (d.UCFDegreeId.HasValue)
            {
                DegreeMapping.Models.Degree ucfDegree = DegreeMapping.Models.Degree.Get(d.UCFDegreeId.Value);
                SetUCFDegreeMap(ucfDegree);
                UCFCourses = DegreeMapping.Models.CourseMap.UCFList(d.UCFDegreeId.Value);
            }
            CourseMap = DegreeMapping.Models.CourseMap.List(degreeId);
            DegreeNotes = DegreeMapping.Models.Note.List(degreeId);
        }

        public void SetUCFDegreeMap(Degree d)
        {
            UCFInstitution = d.Institution;
            UCFGPA = d.GPA;
            UCFDegree = d.Name;
            UCFActive = d.Active;
            UCFLimitedAccess = d.LimitedAccess;
            UCFRestrictedAccess = d.RestrictedAccess;
            UCFDescription = d.Description;
        }
    }
}