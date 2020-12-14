using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace DegreeMapping.Models
{
    public class Database
    {
        public static string DC_DegreeMapping { get { return ConfigurationManager.ConnectionStrings["Connection:DC_DegreeMapping"].ConnectionString; } }
    }
}