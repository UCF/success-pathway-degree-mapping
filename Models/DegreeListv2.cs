using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.DirectoryServices.AccountManagement;

namespace DegreeMapping.Models
{
    public class DegreeListv2
    {
        public int CatalogId { get; set; }
        public string CatalogYear { get; set; }

        public int DegreeId { get; set; }
        public string Degree { get; set; }

        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public int CollegeId { get; set; }
        public string College { get; set; }

        public int UCFDegreeId { get; set; }


        public DegreeListv2()
        {


        }

        public static List<DegreeListv2> List(int? catalogId, int? collegeId, int? degreeId, int? institutionId)
        {
            List<DegreeListv2> list_dl = new List<DegreeListv2>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetDegreeList";
                cmd.CommandType = CommandType.StoredProcedure;
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
                }
                if (collegeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CollegeId", collegeId.Value);
                }
                if (degreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", degreeId.Value);
                }
                if (institutionId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", institutionId.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                { 
                    while(dr.Read())
                    {
                        DegreeListv2 dl = new DegreeListv2();
                        Set(dr, ref dl);
                        list_dl.Add(dl);
                    }
                }
            }
            return list_dl;
        }

        private static void Set(SqlDataReader dr, ref DegreeListv2 dl)
        {
            if (dr.HasRows)
            { 
                dl.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                dl.Degree = dr["Degree"].ToString();
                dl.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                dl.CatalogYear = dr["CatalogYear"].ToString();
                dl.College = dr["College"].ToString();
                dl.CollegeId = Convert.ToInt32(dr["CollegeId"].ToString());
                dl.Institution = dr["Institution"].ToString();
                dl.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
            }
        }
    }
}