using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class BreadCrumb
    {
        public string StartUrl { get; set; }
        public string InstitutionUrl { get; set; }
        public string DegreeUrl { get; set; }

        public BreadCrumb()
        { 
            
        }
        public BreadCrumb(string startUrl, string insitutionUrl, string degreeUrl)
        {
            StartUrl = startUrl;
            InstitutionUrl = insitutionUrl;
            DegreeUrl = degreeUrl;
        }
    }
}