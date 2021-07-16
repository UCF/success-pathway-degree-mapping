﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Migrations.Design;

namespace DegreeMapping.Models
{
    public class UCFCourse
    {
        public string Course { get; set; }
        public int Credit { get; set; }
        public bool Required { get; set; }
        public bool CPP { get; set; }
        public bool Critical { get; set; }

        public UCFCourse()
        { 
        
        }

        public UCFCourse(Course c)
        {
            Course = c.Code;
            Credit = c.Credits;
            Required = c.Required;
            CPP = c.CommonProgramPrerequiste;
            Critical = c.Critical;
        }
    }

    public class PartnerCourse
    {
        public string Course { get; set; }
        public int Credit { get; set; }

        public PartnerCourse()
        { 
        
        }

        public PartnerCourse(Course c)
        {
            Course = c.Code;
            Credit = c.Credits;
        }
    }

    public class CourseMapperJSON
    {
        public int OrderBy { get; set; }

        public string DisplayName { get; set; }
        public List<UCFCourse> UCFCourses { get; set; }
        public List<PartnerCourse> PartnerCourses { get; set; }

        public string AlternateDisplayName { get; set; }
        public List<UCFCourse> AlternateUCFCourse { get; set; }
        public List<PartnerCourse> AlternatePartnerCourse { get; set; }

        public string Alternate2DisplayName { get; set; }
        public List<UCFCourse> Alternate2UCFCourse { get; set; }
        public List<PartnerCourse> Alternate2PartnerCourse { get; set; }

        public string Alternate3DisplayName { get; set; }
        public List<UCFCourse> Alternate3UCFCourse { get; set; }
        public List<PartnerCourse> Alternate3PartnerCourse { get; set; }

        public string Alternate4DisplayName { get; set; }
        public List<UCFCourse> Alternate4UCFCourse { get; set; }
        public List<PartnerCourse> Alternate4PartnerCourse { get; set; }


        public CourseMapperJSON()
        {
            DisplayName = string.Empty;
            UCFCourses = new List<UCFCourse>();
            PartnerCourses = new List<PartnerCourse>();

            AlternateDisplayName = string.Empty;
            AlternateUCFCourse = new List<UCFCourse>();
            AlternatePartnerCourse = new List<PartnerCourse>();

            Alternate2DisplayName = string.Empty;
            Alternate2UCFCourse = new List<UCFCourse>();
            Alternate2PartnerCourse = new List<PartnerCourse>();

            Alternate3DisplayName = string.Empty;
            Alternate3UCFCourse = new List<UCFCourse>();
            Alternate3PartnerCourse = new List<PartnerCourse>();

            Alternate4DisplayName = string.Empty;
            Alternate4UCFCourse = new List<UCFCourse>();
            Alternate4PartnerCourse = new List<PartnerCourse>();
        }

        public CourseMapperJSON(int degreeId)
        {
            DisplayName = string.Empty;
            UCFCourses = new List<UCFCourse>();
            PartnerCourses = new List<PartnerCourse>();

            AlternateDisplayName = string.Empty;
            AlternateUCFCourse = new List<UCFCourse>();
            AlternatePartnerCourse = new List<PartnerCourse>();

            Alternate2DisplayName = string.Empty;
            Alternate2UCFCourse = new List<UCFCourse>();
            Alternate2PartnerCourse = new List<PartnerCourse>();

            Alternate3DisplayName = string.Empty;
            Alternate3UCFCourse = new List<UCFCourse>();
            Alternate3PartnerCourse = new List<PartnerCourse>();

            Alternate4DisplayName = string.Empty;
            Alternate4UCFCourse = new List<UCFCourse>();
            Alternate4PartnerCourse = new List<PartnerCourse>();

            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
            foreach (CourseMapper cm in list_cm)
            {
                #region Primary
                DisplayName = cm.DisplayName;
                foreach (Course c in cm.UCFCourses)
                {
                    UCFCourses.Add(new UCFCourse(c));
                }
                foreach (Course c in cm.PartnerCourses)
                {
                    PartnerCourses.Add(new PartnerCourse(c));
                }
                #endregion
                #region Alternate
                if (cm.AlternateUCFCourses.Count > 0)
                {
                    AlternateDisplayName = cm.AlternateDisplayName;
                    foreach (Course c in cm.AlternateUCFCourses)
                    {
                        AlternateUCFCourse.Add(new UCFCourse(c));
                    }
                }
                if (cm.AlternatePartnerCourses.Count > 0)
                {
                    AlternateDisplayName = cm.AlternateDisplayName;
                    foreach (Course c in cm.AlternatePartnerCourses)
                    {
                        AlternatePartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                #region Alternate 2
                if (cm.Alternate2UCFCourses.Count > 0)
                {
                    Alternate2DisplayName = cm.Alternate2DisplayName;
                    foreach (Course c in cm.Alternate2UCFCourses)
                    {
                        Alternate2UCFCourse.Add(new UCFCourse(c));
                    }
                }
                if (cm.Alternate2PartnerCourses.Count > 0)
                {
                    Alternate2DisplayName = cm.Alternate2DisplayName;
                    foreach (Course c in cm.Alternate2PartnerCourses)
                    {
                        Alternate2PartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                #region Alternate 3
                if (cm.Alternate3UCFCourses.Count > 0)
                {
                    Alternate3DisplayName = cm.Alternate3DisplayName;
                    foreach (Course c in cm.Alternate3UCFCourses)
                    {
                        Alternate3UCFCourse.Add(new UCFCourse(c));
                    }
                }

                if (cm.Alternate3PartnerCourses.Count > 0)
                {
                    Alternate3DisplayName = cm.Alternate3DisplayName;
                    foreach (Course c in cm.Alternate3PartnerCourses)
                    {
                        Alternate3PartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                #region Alternate 4
                if (cm.Alternate4UCFCourses.Count > 0)
                {
                    Alternate4DisplayName = cm.Alternate4DisplayName;
                    foreach (Course c in cm.Alternate4UCFCourses)
                    {
                        Alternate3UCFCourse.Add(new UCFCourse(c));
                    }
                }

                if (cm.Alternate4PartnerCourses.Count > 0)
                {
                    Alternate4DisplayName = cm.Alternate4DisplayName;
                    foreach (Course c in cm.Alternate4PartnerCourses)
                    {
                        Alternate4PartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
            }
        }

        public static List<CourseMapperJSON> List(int degreeId)
        {
            List<CourseMapperJSON> list_cmj = new List<CourseMapperJSON>();
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);

            foreach (CourseMapper cm in list_cm)
            {
                CourseMapperJSON cmj = new CourseMapperJSON();
                #region Primary
                cmj.DisplayName = (cm.DisplayName == CourseMapper.DisplayType.Default) ? string.Empty : cm.DisplayName;
                foreach (Course c in cm.UCFCourses)
                {
                    cmj.UCFCourses.Add(new UCFCourse(c));
                }
                foreach (Course c in cm.PartnerCourses)
                {
                    cmj.PartnerCourses.Add(new PartnerCourse(c));
                }
                #endregion
                #region Alternate
                cmj.AlternateDisplayName = (cm.AlternateDisplayName == CourseMapper.DisplayType.Default) ? string.Empty : cm.AlternateDisplayName;
                if (cm.AlternateUCFCourses.Count > 0)
                {
                    foreach (Course c in cm.AlternateUCFCourses)
                    {
                        cmj.AlternateUCFCourse.Add(new UCFCourse(c));
                    } 
                }
                if (cm.AlternatePartnerCourses.Count > 0)
                {
                    foreach (Course c in cm.AlternatePartnerCourses)
                    {
                        cmj.AlternatePartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                #region Alternate 2
                cmj.Alternate2DisplayName = (cm.Alternate2DisplayName == CourseMapper.DisplayType.Default) ? string.Empty : cm.Alternate2DisplayName;
                if (cm.Alternate2UCFCourses != null && cm.Alternate2UCFCourses.Count > 0)
                {
                    foreach (Course c in cm.Alternate2UCFCourses)
                    {
                        cmj.Alternate2UCFCourse.Add(new UCFCourse(c));
                    }
                }
                if (cm.Alternate2PartnerCourses != null && cm.Alternate2PartnerCourses.Count > 0)
                {
                    foreach (Course c in cm.Alternate2PartnerCourses)
                    {
                        cmj.Alternate2PartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                #region Alternate 3
                cmj.Alternate3DisplayName = (cm.Alternate3DisplayName == CourseMapper.DisplayType.Default) ? string.Empty : cm.Alternate3DisplayName;
                if (cm.Alternate3UCFCourses != null && cm.Alternate3UCFCourses.Count > 0)
                {
                    foreach (Course c in cm.Alternate3UCFCourses)
                    {
                        cmj.Alternate3UCFCourse.Add(new UCFCourse(c));
                    }
                }
                if (cm.Alternate3PartnerCourses != null && cm.Alternate3PartnerCourses.Count > 0)
                {
                    foreach (Course c in cm.Alternate3PartnerCourses)
                    {
                        cmj.Alternate3PartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                #region Alternate 4
                cmj.Alternate4DisplayName = (cm.Alternate4DisplayName == CourseMapper.DisplayType.Default) ? string.Empty : cm.Alternate4DisplayName;
                if (cm.Alternate4UCFCourses != null && cm.Alternate4UCFCourses.Count > 0)
                {
                    foreach (Course c in cm.Alternate4UCFCourses)
                    {
                        cmj.Alternate4UCFCourse.Add(new UCFCourse(c));
                    }
                }
                if (cm.Alternate4PartnerCourses != null && cm.Alternate4PartnerCourses.Count > 0)
                {
                    foreach (Course c in cm.Alternate4PartnerCourses)
                    {
                        cmj.Alternate4PartnerCourse.Add(new PartnerCourse(c));
                    }
                }
                #endregion
                list_cmj.Add(cmj);
            }
            return list_cmj;
        }
    }
}