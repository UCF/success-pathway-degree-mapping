using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class DegreeList
    {
        public int DegreeId { get; set; }
        public string Degree { get; set; }
        public int UCFDegreeId { get; set; }
        public string UCFDegree { get; set; }
        public int InstitutinId { get; set; }
        public string Institution { get; set; }

        public DegreeList()
        { 
            
        
        }

        public static List<DegreeList> GetDegreeList()
        {
            List<DegreeList> list_dl = new List<DegreeList>();
            List<DegreeMapping.Models.Degree> list_d = DegreeMapping.Models.Degree.List(null);
            foreach (DegreeMapping.Models.Degree d in list_d.Where(x=>x.InstitutionId != DegreeMapping.Models.Institution.UCFId))
            {
                if(d.UCFDegreeId.HasValue) 
                {
                    DegreeList dl = new DegreeList();
                    dl.Degree = d.Name;
                    dl.DegreeId = d.Id;
                    dl.Institution = d.Institution;
                    dl.InstitutinId = d.InstitutionId;
                    dl.UCFDegree = d.UCFDegreeName;
                    dl.UCFDegreeId = d.UCFDegreeId.Value;
                    list_dl.Add(dl);
                }
            }
            return list_dl;
        }
    }
}