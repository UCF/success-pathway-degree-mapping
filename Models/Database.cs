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

        public static bool IsProduction { get { return IsProductionDatabase(Database.DC_DegreeMapping); } }

        private static bool IsProductionDatabase(string connectionString)
        {
            string dbname = "UCN_Degree_Mapping_DEV";
            if (connectionString.ToLower().Contains(dbname.ToLower()))
            {
                return false;
            }
            return true;
        }
    }
}