using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Xml.Linq;

namespace DegreeMapping.Models
{
    public class DegreeList
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string DegreeType { get; set; }
        public int InstitutionId { get; set; }
        public string Institution { get; set; }
        public string GPA { get; set; }
        public bool LimitedAccess { get; set; }
        public bool RestrictedAccess { get; set; }
        public List<DegreeList> Degrees {get; set; }
        public List<CourseList> Courses { get; set; }
        public List<NoteList> Notes { get; set; }
        public List<Generic> Generic { get; set; }

        public DegreeList()
        {
            Degrees = new List<DegreeList>();
            Notes = new List<NoteList>();
            Courses = new List<CourseList>();
            Generic = new List<Generic>();
        }

        public DegreeList(Degree d)
        {
            Id = d.Id;
            Degree = d.Name;
            DegreeType = d.DegreeType;
            this.InstitutionId = d.InstitutionId;
            this.Institution = d.Institution;
            this.GPA = d.GPA;
            this.LimitedAccess = d.LimitedAccess;
            this.RestrictedAccess = d.RestrictedAccess;
            Degrees = new List<DegreeList>();
            Notes = new List<NoteList>();
            Courses = new List<CourseList>();
            Generic = new List<Generic>();
        }

        public static List<DegreeList> List()
        {
            List<DegreeList> list_dl = new List<DegreeList>();
            List<Degree> list_d = Models.Degree.List(DegreeMapping.Models.Institution.UCFId, null);
            List<Degree> list_d2 = Models.Degree.List(null,null);
            foreach (Degree d in list_d.OrderBy(x => x.Name))
            {
                DegreeList dl = new DegreeList(d);
                dl.Courses = CourseList.List(d.Id);
                dl.Notes = NoteList.List(d.Id);
                foreach(Degree d2 in list_d2.Where(x=>x.UCFDegreeId==d.Id))
                {
                    DegreeList dl2 = new DegreeList();
                    dl2.Id = d2.Id;
                    dl2.Degree = d2.Name;
                    dl2.DegreeType = d2.DegreeType;
                    dl2.InstitutionId = d2.InstitutionId;
                    dl2.Institution = d2.Institution;
                    dl2.Courses = CourseList.List(dl2.Id);
                    dl2.Notes = NoteList.List(dl2.Id);
                    dl.Degrees.Add(dl2);
                }
                list_dl.Add(dl);
            }
            return list_dl;
        }
    }
}