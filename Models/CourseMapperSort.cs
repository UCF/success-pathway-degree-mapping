using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DegreeMapping.Models
{
    public class CourseMapperSort
    {



        public static void SetAllSort()
        {
            List<int> list_degreeIds = CourseMapper.List(null, null, null).Select(x => x.DegreeId).Distinct().ToList();

            foreach (int degreeId in list_degreeIds)
            {
                int sortOrderNumber = 1000;
                List<CourseMapper> list_cm = CourseMapper.List(degreeId, null, null);
                foreach (CourseMapper cm in list_cm)
                {
                    cm.SortOrder = sortOrderNumber++;
                    CourseMapper.Update(cm);
                }
            }
        }

        /// <summary>
        /// used to reset CourseMapper SortOrder
        /// </summary>
        public static void UpdateOrderBy(int degreeId)
        {
            List<int> list_Id = CourseMapper.GetCourseMapperDistinctDegreeId();
            //foreach (int degreeId in list_Id)
            //{
                int orderNumber = 1000;
                List<CourseMapper> list_cm = CourseMapper.List(degreeId, null, null);
                foreach (CourseMapper cm in list_cm)
                {
                    CourseMapper.UpdateSortOrder(cm.Id, orderNumber);
                    orderNumber = orderNumber + 1;
                }
            //}
        }
    }
}