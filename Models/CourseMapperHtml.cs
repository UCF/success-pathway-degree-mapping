using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class CourseMapperHtml
    {
        public int DegreeId { get; set; }
        public int DisplayValue { get; set; }
        public List<Course> List_UCFCourses { get; set; }
        public List<Course> List_PartnerCourses { get; set; }

        public string HTMLDisplayValue { get; set; }
        public string HTMLUCFCourseIds { get; set; }
        public string HTMLPartnerCourseIds { get; set; }

        public CourseMapperHtml()
        {
            List_UCFCourses = new List<Course>();
            List_PartnerCourses = new List<Course>();
        }

        public CourseMapperHtml(int degreeId, int displayValue, List<Course> list_ucfCourses, List<Course> list_partnerCourses, string htmlDisplayValue, string htmlUCFCourseIds, string htmlPartnerCourseIds) 
        {
            List_UCFCourses = new List<Course>();
            List_PartnerCourses = new List<Course>();

            DegreeId = degreeId;
            DisplayValue = displayValue;
            List_UCFCourses = list_ucfCourses;
            List_PartnerCourses = list_partnerCourses;
            HTMLDisplayValue = htmlDisplayValue;
            HTMLUCFCourseIds = htmlUCFCourseIds;
            HTMLPartnerCourseIds = htmlPartnerCourseIds;
        }
    }
}