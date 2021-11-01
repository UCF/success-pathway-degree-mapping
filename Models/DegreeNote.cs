using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System.Web.Management;

namespace DegreeMapping.Models
{
    public class DegreeNote
    {
        public int Id { get; set; }
        public int DegreeId { get; set; }
        [DisplayName("Title")]
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
        [DisplayName("Show title on website")]
        public bool ShowName { get; set; }
        [DisplayName("Display Order")]
        public int OrderBy { get; set; }
        public bool Active { get; set; }
        public int Section { get; set; }
        public bool ForeignLanguageRequirement { get; set; }

        public DegreeNote()
        { 
        
        }

        public DegreeNote(DegreeMapping.Models.Note n)
        {
            Id = n.Id;
            DegreeId = n.DegreeId;
            Name = n.Name;
            Value = n.Value;
            Required = n.Required;
            ShowName = n.ShowName;
            OrderBy = n.OrderBy;
            Active = n.Active;
            Section = n.Section;
            ForeignLanguageRequirement = n.ForeignLanguageRequirement;
        }

        public static List<DegreeNote> List(int degreeId)
        {
            List<DegreeNote> list_dn = new List<DegreeNote>();
            List<DegreeMapping.Models.Note> list_n = DegreeMapping.Models.Note.List(degreeId, null).Where(x => x.Active).ToList();
            foreach(DegreeMapping.Models.Note n in list_n.OrderBy(x=>x.OrderBy).ThenBy(x=>x.Name)) 
            {
                DegreeNote dn = new DegreeNote(n);
                list_dn.Add(dn);
            }
            return list_dn;
        }
    }
}