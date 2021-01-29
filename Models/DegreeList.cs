using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class DegreeList
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public int InstitutionId { get; set; }
        public string Institution { get; set; }
        public List<DegreeList> Degrees {get; set; }

        public DegreeList()
        {
            Degrees = new List<DegreeList>();
        }

        public DegreeList(Degree d)
        {
            Id = d.Id;
            Degree = d.Name;
            this.InstitutionId = d.InstitutionId;
            this.Institution = d.Institution;
            Degrees = new List<DegreeList>();
        }

        public static List<DegreeList> List()
        {
            List<DegreeList> list_dl = new List<DegreeList>();
            List<Degree> list_d = Models.Degree.List(DegreeMapping.Models.Institution.UCFId);
            List<Degree> list_d2 = Models.Degree.List(null);
            foreach (Degree d in list_d.OrderBy(x => x.Name))
            {
                DegreeList dl = new DegreeList(d);
                foreach(Degree d2 in list_d2.Where(x=>x.UCFDegreeId==d.Id))
                {
                    DegreeList dl2 = new DegreeList();
                    dl2.Id = d2.Id;
                    dl2.Degree = d2.Name;
                    dl2.InstitutionId = d2.InstitutionId;
                    dl2.Institution = d2.Institution;
                    dl.Degrees.Add(dl2);
                }
                list_dl.Add(dl);
            }
            return list_dl;
        }
    }
}