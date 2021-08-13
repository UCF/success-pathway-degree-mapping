using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace DegreeMapping.Models
{
    public class Note
    {
        public struct NoteTypeValue
        {
            public static int Note { get { return 1;  } }
            public static int ListItem { get { return 2; } }
            public static int ForeignLanguageRequirement { get { return 3; } }
            public static int AdditionalRequirement { get { return 4; } }
        }

        public struct Message
        {
            public static string Get(string institution)
            {
                return (institution == "UCF") ? Note.Message.ForUCF : Note.Message.ForPartner(institution);
            }
            private static string ForUCF { get { return "Global note for all institutions for this degree."; } }
            private static string ForPartner(string institution)
            {
                return "This note will only display for " + institution;
            } 
        }

        public int Id { get; set; }
        public int DegreeId { get; set; }
        [DisplayName("Title")]
        public string Name { get; set; }
        [DisplayName("Content")]
        public string Value { get; set; }
        public bool Required { get; set; }
        [DisplayName("Show title on website")]
        public bool ShowName { get; set; }
        [DisplayName("Display Order")]
        public int OrderBy { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public bool Active { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int InstitutionId { get; set; }
        [DisplayName("Foreign Language Requirement")]
        public bool ForeignLanguageRequirement { get; set; }
        [DisplayName("Display Section")]
        public int Section { get; set; }
        [DisplayName("Note Type")]
        public int NoteType { get; set; }
        public int CatalogyId { get; set; }
        [DisplayName("Catalog Year")]
        public string CatalogYear { get; set; }
        public int? CloneNoteId { get; set; }

        public Note()
        {
            DegreeId = 0;
            Required = false;
            Active = true;
            NID = HttpContext.Current.User.Identity.Name;
            Section = 1;
            ShowName = false;
            OrderBy = 1;
            NoteType = Note.NoteTypeValue.Note;
        }

        public Note(int degreeId)
        {
            Degree d = DegreeMapping.Models.Degree.Get(degreeId);
            Active = true;
            this.Degree = d.Name;
            DegreeId = d.Id;
            this.Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            Required = true;
            Active = true;
            NID = HttpContext.Current.User.Identity.Name;
            Section = 1;
            ShowName = false;
            OrderBy = 1;
            NoteType = Note.NoteTypeValue.Note;
            CatalogYear = d.CatalogYear;
            CatalogyId = d.CatalogId;

        }

        public static int Insert(Note n)
        {
            int id = 0;
            n.Name = (!string.IsNullOrEmpty(n.Name)) ? n.Name : "Note";
            n.Value = (!string.IsNullOrEmpty(n.Value)) ? n.Value : "Empty";
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "InsertNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", n.DegreeId);
                cmd.Parameters.AddWithValue("@Name", n.Name);
                cmd.Parameters.AddWithValue("@Value", n.Value);
                cmd.Parameters.AddWithValue("@Required", n.Required);
                cmd.Parameters.AddWithValue("@Active", n.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", n.NID);
                cmd.Parameters.AddWithValue("@ShowName", n.ShowName);
                cmd.Parameters.AddWithValue("@OrderBy", n.OrderBy);
                cmd.Parameters.AddWithValue("@ForeignLanguageRequirement", n.ForeignLanguageRequirement);
                cmd.Parameters.AddWithValue("@Section", n.Section);
                cmd.Parameters.AddWithValue("@NoteType", n.NoteType);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return id;
        }

        public static void Update(Note n)
        {
            n.Name = (!string.IsNullOrEmpty(n.Name)) ? n.Name : "Note";
            n.Value = (!string.IsNullOrEmpty(n.Value)) ? n.Value : "Empty";
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", n.Id);
                cmd.Parameters.AddWithValue("@DegreeId", n.DegreeId);
                cmd.Parameters.AddWithValue("@Name", n.Name);
                cmd.Parameters.AddWithValue("@Value", n.Value);
                cmd.Parameters.AddWithValue("@Required", n.Required);
                cmd.Parameters.AddWithValue("@Active", n.Active);
                cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@NID", n.NID);
                cmd.Parameters.AddWithValue("@ShowName", n.ShowName);
                cmd.Parameters.AddWithValue("@OrderBy", n.OrderBy);
                cmd.Parameters.AddWithValue("@ForeignLanguageRequirement", n.ForeignLanguageRequirement);
                cmd.Parameters.AddWithValue("@Section", n.Section);
                cmd.Parameters.AddWithValue("@NoteType", n.NoteType);
                //if (n.CloneNoteId.HasValue) {
                //    cmd.Parameters.AddWithValue("@CloneNoteId", n.CloneNoteId.Value);
                //}
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static List<Note> List(int degreeId)
        {
            List<Note> list_n = new List<Note>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DegreeId", degreeId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Note n = new Note();
                        Set(dr, ref n);
                        list_n.Add(n);
                    }
                }
                cn.Close();
            }
            return list_n.OrderBy(x=>x.OrderBy).ThenBy(x=>x.Name).ToList();
        }

        public static Note Get(int id)
        {
            Note n = new Note();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref n);
                    }
                }
                cn.Close();
            }
            return n;
        }

        private static void Set(SqlDataReader dr, ref Note n)
        {
            if (dr.HasRows)
            {
                n.Id = Convert.ToInt32(dr["Id"].ToString());
                n.Name = dr["Name"].ToString();
                n.Value = dr["Value"].ToString();
                n.Required = Convert.ToBoolean(dr["Required"].ToString());
                n.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                n.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                n.NID = dr["NID"].ToString();
                n.Active = Convert.ToBoolean(dr["Active"].ToString());
                n.Degree = dr["Degree"].ToString();
                n.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                n.Institution = dr["Institution"].ToString();
                n.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                n.ShowName = Convert.ToBoolean(dr["ShowName"].ToString());
                n.OrderBy = Convert.ToInt32(dr["OrderBy"].ToString());
                n.ForeignLanguageRequirement = Convert.ToBoolean(dr["ForeignLanguageRequirement"].ToString());
                n.Section = Convert.ToInt32(dr["Section"].ToString());
                n.NoteType = Convert.ToInt32(dr["NoteType"].ToString());
                n.CatalogYear = dr["CatalogYear"].ToString();
                n.CatalogyId = Convert.ToInt32(dr["CatalogId"].ToString());
                //int cloneNoteId;
                //Int32.TryParse(dr["CloneNoteId"].ToString(), out cloneNoteId);
                //n.CloneNoteId = cloneNoteId;
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "DeleteNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        public static string GetNoteTypeValue(int noteTypeValue)
        {
            switch (noteTypeValue) {
                case 4: return "Additional Requirements";
                case 3: return "Foreign Langauge Requirement";
                case 2: return "List Item";
                default: return "Note";
            }
        }
    }
}