using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapping.Models
{
    public class FavoriteDegree
    {
        public string NID { get; set; }
        public List<int> DegreeList { get; set; } = new List<int>();
        public List<Degree> List_Degrees { get; set; } = new List<Degree>();
        private bool hasRecord { get; set; } = false;

        public FavoriteDegree(string nid)
        {
            NID = nid;
        }

        public static void Insert(FavoriteDegree fd) 
        {
            FavoriteDegree existingFD = Get(fd.NID);
            if (fd.hasRecord)
            {
                Update(fd);
                return;
            }
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertFavoriteDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", fd.NID);
                cmd.Parameters.AddWithValue("@DegreeList", string.Join(",", fd.DegreeList));
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static void Update(FavoriteDegree fd)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateFavoriteDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", fd.NID);
                cmd.Parameters.AddWithValue("@DegreeList", string.Join(",", fd.DegreeList));
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static FavoriteDegree Get(string NID)
        {
            FavoriteDegree fd = new FavoriteDegree(NID);
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetFavoriteDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NID", NID);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows) 
                {
                    fd.hasRecord = true;
                    while (dr.Read())
                    {
                        Set(dr, ref fd);
                    }
                }
                cn.Close();
            }
            return fd;
        }

        private static void Set(SqlDataReader dr, ref FavoriteDegree fd)
        {
            if (dr.HasRows)
            {
                fd.NID = dr["NID"].ToString();
                string degreeList = dr["DegreeList"].ToString();
                if (!string.IsNullOrEmpty(degreeList))
                {
                    fd.DegreeList = dr["DegreeList"].ToString().Split(',').Select(Int32.Parse).ToList();
                }
                SetDegree(ref fd);
            }
        }

        private static void SetDegree(ref FavoriteDegree fd) 
        {
            if (fd != null && fd.DegreeList.Count > 0) 
            {
                foreach (int id in fd.DegreeList)
                {
                    Degree d = Degree.Get(id);
                    fd.List_Degrees.Add(d);
                }
            }
        }

    }
}