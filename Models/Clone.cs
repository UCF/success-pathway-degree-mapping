using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Scope;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Channels;
using System.EnterpriseServices;

namespace DegreeMapping.Models
{
    public class Clone
    {

        #region Clone Degree
        /// <summary>
        /// STEP 1
        /// CLONE UCF Degrees to new catalog
        /// </summary>
        /// <param name="sourceCatalogId"></param>
        /// <param name="targetCatalogId"></param>
        public static void CloneUCFDegrees(int sourceCatalogId, int targetCatalogId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping)) 
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "CloneUCFDegrees";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceCatalogId", sourceCatalogId);
                cmd.Parameters.AddWithValue("@TargetCatalogId", targetCatalogId);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        /// <summary>
        /// STEP 2A
        /// LOOP THROUGH SOURCE CATALOG THAT ARE NOT PART OF UCF
        /// </summary>
        /// <param name="sourceCatalogId"></param>
        /// <param name="targetCatalogId"></param>
        public static void ClonePartnerDegrees(int sourceCatalogId, int targetCatalogId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "ClonePartnerDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceCatalogId", sourceCatalogId);
                cmd.Parameters.AddWithValue("@TargetCatalogId", targetCatalogId);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }

        /// <summary>
        /// Step 2C
        /// Update the UCFDegreeId to the new DegreeId
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateUCFDegreeId(int id)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateUCFDegreeId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }
        #endregion

        #region Clone Course
        /// <summary>
        /// Step 3 Clone courses
        /// -Get all current catalog courses
        /// -Loop through courses
        /// -Get original degreeId (from Degree)
        /// -Get new degreeid from cloned Degree
        /// -copy course, update degreeid
        /// -insert new course
        /// </summary>
        /// <param name="sourceCatalogId"></param>
        public static void CloneCourses(int sourceCatalogId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "CloneCourses";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sourceCatalogId", sourceCatalogId);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }
        public static void UpdateUCFCourseId(int targetCatalogId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateUCFCourseId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TargerCatalogId", targetCatalogId);
                cmd.ExecuteScalar();
                cn.Close();
            }

        }


        /// <summary>
        /// NO LONGER NEEDED
        /// </summary>
        /// <param name="sourceCatalogId"></param>
        /// <param name="targetCatalogId"></param>
        public static void UpdateCourseDegreeId(int sourceCatalogId, int targetCatalogId)
        {
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "UpdateCourseDegreeId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceCatalogId", sourceCatalogId);
                cmd.Parameters.AddWithValue("@TargerCatalogId", targetCatalogId);
                cmd.ExecuteScalar();
                cn.Close();
            }
        }


        #endregion

        #region Clone CourseMapping
        public static void CloneCourseMapping(int sourceCatalogId) 
        {
            List<CourseMapper> list_cm = CourseMapper.List(null, null, sourceCatalogId);
            Course clonedCourse = new Course();
            foreach (CourseMapper cm in list_cm) 
            {
                Degree newDegree = Degree.GetClonedDegree(cm.DegreeId);
                if (newDegree == null || newDegree.Id < 1) {
                    //Break;
                    continue;
                }

                CourseMapper newCM = new CourseMapper();
                newCM = cm;
                newCM.CloneCourseMapperId = cm.Id;
                newCM.DegreeId = newDegree.Id;
                #region UCFCourseIds
                if (cm != null && cm.UCFCourseIds.Count > 0) 
                {
                    newCM.UCFCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.UCFCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.UCFCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Partner CourseIds
                if (cm != null && cm.PartnerCourseIds.Count > 0)
                {
                    newCM.PartnerCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.PartnerCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.PartnerCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Alternate 1 UCFCourseIds
                if (cm.AlternateUCFCourseIds != null && cm.AlternateUCFCourseIds.Count > 0)
                {
                    newCM.AlternateUCFCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.AlternateUCFCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.AlternateUCFCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Alternate 1 PartnerCourseIds
                if (cm.AlternatePartnerCourseIds != null && cm.AlternatePartnerCourseIds.Count > 0)
                {
                    newCM.AlternatePartnerCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.AlternatePartnerCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.AlternatePartnerCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion

                #region Alternate 2 UCFCourseIds
                if (cm.Alternate2UCFCourseIds != null && cm.Alternate2UCFCourseIds.Count > 0)
                {
                    newCM.Alternate2UCFCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate2UCFCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate2UCFCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Alternate 2 PartnerCourseIds
                if (cm.Alternate2PartnerCourseIds != null && cm.Alternate2PartnerCourseIds.Count > 0)
                {
                    newCM.Alternate2PartnerCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate2PartnerCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate2PartnerCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion

                #region Alternate 3 UCFCourseIds
                if (cm.Alternate3UCFCourseIds != null && cm.Alternate3UCFCourseIds.Count > 0)
                {
                    newCM.Alternate3UCFCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate3UCFCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate3UCFCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Alternate 3 PartnerCourseIds
                if (cm.Alternate3PartnerCourseIds != null && cm.Alternate3PartnerCourseIds.Count > 0)
                {
                    newCM.Alternate3PartnerCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate3PartnerCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate3PartnerCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion

                #region Alternate 4 UCFCourseIds
                if (cm.Alternate4UCFCourseIds != null && cm.Alternate4UCFCourseIds.Count > 0)
                {
                    newCM.Alternate4UCFCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate4UCFCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate4UCFCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Alternate 4 PartnerCourseIds
                if (cm.Alternate4PartnerCourseIds != null && cm.Alternate4PartnerCourseIds.Count > 0)
                {
                    newCM.Alternate4PartnerCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate4PartnerCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate4PartnerCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion

                #region Alternate 5 UCFCourseIds
                if (cm.Alternate5UCFCourseIds != null && cm.Alternate5UCFCourseIds.Count > 0)
                {
                    newCM.Alternate5UCFCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate5UCFCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate5UCFCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
                #region Alternate 5 PartnerCourseIds
                if (cm.Alternate5PartnerCourseIds != null && cm.Alternate5PartnerCourseIds.Count > 0)
                {
                    newCM.Alternate5PartnerCourseIds = new List<int>();
                    foreach (int oldCourseId in cm.Alternate5PartnerCourseIds)
                    {
                        clonedCourse = Course.GetClonedCourse(oldCourseId);
                        if (clonedCourse != null && clonedCourse.Id > 0)
                        {
                            newCM.Alternate5PartnerCourseIds.Add(clonedCourse.Id);
                        }
                    }
                }
                #endregion
            }
        }
        #endregion

        #region Clone Notes
        public static void CloneNote(int sourceCatalodId)
        {
            List<Note> list_n = Note.List(null, sourceCatalodId);
            foreach (Note sourceNote in list_n)
            {
                Note newNote = new Note();
                newNote = sourceNote;
                Degree newDegree = Degree.GetClonedDegree(sourceNote.Id);
                newNote.DegreeId = newDegree.Id;
                newNote.CloneNoteId = sourceNote.Id;
                newNote.UpdateDate = DateTime.Now;
                newNote.AddDate = DateTime.Now;
                Note.Insert(newNote);
            }

        }
        #endregion

        #region Clone Custom Course Semester
        public static void CloneCustomCourseSemester(int sourceCatalogId)
        {
            List<CustomCourseSemester> list_ccs = CustomCourseSemester.List(null, sourceCatalogId);
            foreach (CustomCourseSemester ccs in list_ccs)
            {
                CustomCourseSemester newCCS = new CustomCourseSemester();
                newCCS = ccs;
                Degree newDegree = Degree.GetClonedDegree(ccs.DegreeId);
                newCCS.DegreeId = newDegree.Id;
                CustomCourseSemester.Insert(newCCS);
            }
        }
        #endregion

        #region Clone Custom Course Mapping
        public static void CloneCustomCourseMapper(int sourceCatalogId)
        {
            List<CustomCourseMapper> list_ccm = CustomCourseMapper.List(sourceCatalogId);
            foreach (CustomCourseMapper ccm in list_ccm)
            {
                Degree newDegree = Degree.GetClonedDegree(ccm.DegreeId);
                CustomCourseMapper newCCM = new CustomCourseMapper(newDegree.Id);
                foreach (int id in ccm.List_CourseIds)
                {
                    Course newCourse = Course.GetClonedCourse(id);
                    newCCM.List_CourseIds.Add(newCourse.Id);
                }
                CustomCourseMapper.Insert(newCCM);
            }
        }
        #endregion
    }
}