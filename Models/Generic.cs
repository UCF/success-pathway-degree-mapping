using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class Generic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institution { get; set; }
        public Generic()
        { 
        
        }

        public Generic(Degree d)
        {
            Id = d.Id;
            Name = d.Name;
            Institution = d.Institution;

        }

        public static List<Generic> List()
        {
            List<Generic> list_g = new List<Generic>();
            List<Degree> list_d = Degree.List(DegreeMapping.Models.Institution.GenericId);
            foreach (Degree d in list_d.Where(x=>x.Active).OrderBy(x=>x.Name))
            {
                list_g.Add(new Generic(d));
            }
            return list_g;
        }

        public static List<Generic> List(int UCFDegreeId)
        {
            List<Generic> list_g = new List<Generic>();
            List<Degree> list_d = Degree.List(null);
            foreach (Degree d in list_d.OrderBy(x=>x.Name).Where(x=>x.Active && x.UCFDegreeId == UCFDegreeId))
            {
                list_g.Add(new Generic(d));
            }
            return list_g;
        }
    }
}