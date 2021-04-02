using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class DegreeMap
    {
        #region Properties
        public int Id { get; set; }
        public string Degree { get; set; }
        public string DegreeType { get; set; }
        public int InstitutionId { get; set; }
        public string Institution { get; set; }
        public string GPA { get; set; }
        public bool LimitedAccess { get; set; }
        public bool RestrictedAccess { get; set; }
        public bool HasUCFSemesters { get; set; }
        public List<CourseList> Courses { get; set; }
        public List<NoteList> Notes { get; set; }
        public List<Generic> Generic { get; set; }
        #endregion


        public DegreeMap()
        {
            HasUCFSemesters = false;
            Notes = new List<NoteList>();
            Courses = new List<CourseList>();
            Generic = new List<Generic>();
        }

        public DegreeMap(Degree d)
        {
            Id = d.Id;
            Degree = d.Name;
            DegreeType = d.DegreeType;
            this.InstitutionId = d.InstitutionId;
            this.Institution = d.Institution;
            this.GPA = d.GPA;
            this.LimitedAccess = d.LimitedAccess;
            this.RestrictedAccess = d.RestrictedAccess;
            HasUCFSemesters = false;
            Notes = new List<NoteList>();
            Courses = new List<CourseList>();
            Generic = new List<Generic>();
        }

        public static DegreeMap Get(int degreeId)
        {
            DegreeMapping.Models.Degree degree = DegreeMapping.Models.Degree.Get(degreeId);
            DegreeMap dm = new DegreeMap(degree);
            List<NoteList> list_nl = new List<NoteList>();
            List<Note> list_n = Note.List(degreeId);
            if (list_n.Count > 0)
            {
                foreach (Note n in list_n.Where(x => x.NoteType == Note.NoteTypeValue.ListItem))
                {
                    NoteList nl = new NoteList(n);
                    dm.Notes.Add(nl);
                }
            }
            List<CourseList> list_cl = CourseList.List(degreeId);
            dm.Courses = list_cl;
            if (degree.UCFDegreeId.HasValue)
            {
                dm.Generic = DegreeMapping.Models.Generic.List(degree.UCFDegreeId.Value);
                DegreeMapping.Models.Degree ucfDegree = DegreeMapping.Models.Degree.Get(degree.UCFDegreeId.Value);
                DegreeMap dm2 = new DegreeMap(ucfDegree);
                dm.GPA = dm2.GPA;
                dm.RestrictedAccess = dm2.RestrictedAccess;
                dm.LimitedAccess = dm2.LimitedAccess;
                if (CourseList.List(degree.UCFDegreeId.Value).Where(x => x.Semester > 1).ToList().Count() > 0)
                {
                    dm.HasUCFSemesters = true;
                    dm.Courses.AddRange(CourseList.List(degree.UCFDegreeId.Value).Where(x => x.Semester > 1).ToList());
                }
                dm.Notes.AddRange(NoteList.List(degree.UCFDegreeId.Value));

            }
            return dm;
        }
    }
}