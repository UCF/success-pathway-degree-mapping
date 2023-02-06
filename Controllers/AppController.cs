using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DegreeMapping.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;
using Microsoft.Win32;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations;

namespace DegreeMapping.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        // GET: App
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult _Dashboard()
        {
            return PartialView();
        }

        public ActionResult _CatalogDashboard()
        {
            List<DegreeMapping.Models.Catalog> list_catalog = DegreeMapping.Models.Catalog.List();
            return PartialView(list_catalog);
        }

        public ActionResult DegreeList(int id, string keyword)
        {
            List<DegreeMapping.Models.Degree> list_d = DegreeMapping.Models.Degree.List(null, id);
            ViewBag.Keyword = string.Empty;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ViewBag.JSONDegreeList = jss.Serialize(list_d);
            return View(list_d);
        }

        #region Institutions
        public ActionResult Institutions()
        {
            List<Institution> list_i = Institution.List();
            return View(list_i);
        }

        public ActionResult InstitutionView(int id)
        {
            Institution i = Institution.Get(id);
            return View(i);
        }

        public ActionResult InstitutionEdit(int id)
        {
            Institution i = Institution.Get(id);
            return View(i);
        }

        public ActionResult InstitutionAdd()
        {
            Institution i = new Institution();
            return View(i);
        }

        [HttpPost]
        public ActionResult InstitutionSave(Institution i)
        {
            if (i.Id > 0)
            {
                Institution.Update(i);
            }
            else
            {
                int id = Institution.Insert(i);
                i.Id = id;
            }
            return RedirectToAction("InstitutionView", new { id = i.Id });
        }

        public ActionResult _Institution(Institution model)
        {
            return PartialView(model);
        }
        #endregion

        #region Degrees
        public ActionResult DegreeAdd(int institutionId, int catalogId, int? collegeId, int? ucfDegreeId)
        {
            Degree d = new Degree(institutionId, catalogId);
            d.CollegeId = (collegeId.HasValue) ? collegeId.Value : 0;
            d.UCFDegreeId = (ucfDegreeId.HasValue) ? ucfDegreeId.Value : 0;
            return View(d);
        }

        public ActionResult DegreeEdit(int id)
        {
            Degree d = Degree.Get(id);
            return View(d);
        }
        public ActionResult DegreeView(int id)
        {
            Degree d = Degree.Get(id);
            return View(d);
        }

        [HttpPost]
        public ActionResult DegreeSave(Degree d)
        {
            d.Active = (d.Active) ? d.Active : false;
            if (d.Id > 0)
            { 
                Degree.Update(d);
            }
            else
            {
                int id = Degree.Insert(d);
                d.Id = id;
            }
            return RedirectToAction("DegreeView", new { id = d.Id });
        }

        public ActionResult _Degree(Degree model)
        {
            return PartialView(model);
        }

        public ActionResult _DisplayRelatedDegrees(Degree model)
        {
            return PartialView(model);
        }

        /// <summary>
        /// Global Course Notes is stored in the Degree Table
        /// DegreeId is the UCF Degree Id NOT the Partner DegreeId
        /// </summary>
        /// <returns></returns>
        public ActionResult _DisplayGlobalCourseNotes(string globalCourseNotes, int degreeId)
        {
            ViewBag.GlobalCourseNotes = globalCourseNotes;
            ViewBag.DegreeId = degreeId;
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateGlobalCourseNotes(string globalCourseNotes, int degreeId)
        {
            Degree.UpdateGlobalCourseNotes(globalCourseNotes, degreeId);
            string updateGlobalCourseNotes = Degree.Get(degreeId).GlobalCourseNotes;
            return Json(new { globalCourseNotes = updateGlobalCourseNotes, degreeId = degreeId }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// February 2023
        /// New feature 
        /// Update the DisplayMultipleSemesters
        /// </summary>
        /// <param name="displatMultipleSemesters"></param>
        /// <param name="degreeId"></param>
        /// <returns></returns>
        public ActionResult UpdateDisplatMultipleSemesters(bool displatMultipleSemesters, int degreeId)
        {
            displatMultipleSemesters = Degree.UpdateDisplayMultipleSemesters(displatMultipleSemesters, degreeId);
            return Json(new { displatMultipleSemesters = displatMultipleSemesters }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region NOTE USE | Code - Updates course code only
        public ActionResult CodeList()
        {
            List<Course> list_c = Course.List(null, null);
            return View(list_c);
        }
        public ActionResult _CodeEdit(Course c)
        {
            return PartialView(c);
        }

        [HttpPost]
        public ActionResult _CodeSave(Course c)
        {
            //Course.Update(c);
            return RedirectToAction("CodeList");
        }
        #endregion

        #region Courses

        public ActionResult CourseAdd(int degreeId)
        {
            Course c = new Course(degreeId);
            return View(c);
        }

        public ActionResult CourseEdit(int id)
        {
            Course c = Course.Get(id);
            return View(c);
        }

        public ActionResult CourseView(int id)
        {
            Course c = Course.Get(id);
            return View(c);
        }

        public ActionResult CourseList()
        {
            List<Course> list_course = Course.List(null, null);
            return View(list_course);
        }

        public ActionResult CourseSearch(string keyword, int? catalogId)
        {
            List<Course> list_c = new List<Course>();
            if (!string.IsNullOrEmpty(keyword))
            {
                if (catalogId.HasValue) {
                    list_c = Course.Search(keyword, catalogId.Value);
                } 
                else
                {
                    list_c = Course.Search(keyword, null);
                }
                
            }
            ViewBag.Keyword = keyword;
            ViewBag.CatalogId = catalogId;
            return View(list_c);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CourseSave(Course c)
        {
            if (c.Id > 0)
            {
                Course.Update(c);
            } 
            else
            {
                int id = Course.Insert(c);
                c.Id = id;
            }
            return RedirectToAction("DegreeView", new { id =c.DegreeId });
        }

        public ActionResult CourseDelete(int id, int degreeId)
        {
            
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null, null);
            foreach (CourseMapper cm in list_cm)
            {
                List<int> new_course_ids = new List<int>();
                #region remove Partner courses
                foreach (int courseId in cm.PartnerCourseIds)
                {
                    if (courseId != id)
                    {
                        new_course_ids.Add(courseId);
                    }
                }
                cm.PartnerCourseIds = new_course_ids;
                #endregion

                #region remove UCF Courses
                new_course_ids = new List<int>();
                foreach (int courseId in cm.UCFCourseIds)
                {
                    if (courseId != id)
                    {
                        new_course_ids.Add(courseId);
                    }
                }
                cm.UCFCourseIds = new_course_ids;
                #endregion
                CourseMapper.Update(cm);
            }
            list_cm = CourseMapper.List(degreeId, null, null);
            if (list_cm.Select(x=>x.PartnerCourseIds).Count() == 0 && list_cm.Select(x=>x.UCFCourseIds).Count()==0)
            {
                int courseMapperId = list_cm.Select(x => x.Id).FirstOrDefault();
                CourseMapper.Delete(courseMapperId);
            }
            Course.Delete(id);
            return RedirectToAction("DegreeView", new { id = degreeId });
        }

        public ActionResult _Course(Course model)
        {
            return PartialView(model);
        }

        #endregion

        #region Notes
        public ActionResult NoteView(int id)
        {
            Note n = Note.Get(id);
            return View(n);
        }

        public ActionResult NoteAdd(int degreeId)
        {
            Note n = new Note(degreeId);
            return View(n);
        }

        public ActionResult NoteEdit(int id)
        {
            Note n = Note.Get(id);
            return View(n);
        }

        public ActionResult NoteDelete(int id, int degreeId)
        {
            Note.Delete(id);
            return RedirectToAction("DegreeView", new { id = degreeId});
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NoteSave(Note n)
        {
            if (n.Id > 0)
            {
                Note.Update(n);
            }
            else 
            {
                int id = Note.Insert(n);
                n.Id = id;
            }
            return RedirectToAction("DegreeView", "App", new { id = n.DegreeId });
        }

        public ActionResult _Note(Note model)
        {
            return PartialView(model);
        }
        #endregion

        #region Users
        public ActionResult ViewUsers()
        {
            if (!User.IsInRole(DegreeMapping.Models.Role.Admin))
            {
                return RedirectToAction("Catalog", "App");
            }
            List<User> list_users = DegreeMapping.Models.User.List();
            return View(list_users);
        }

        [HttpPost]
        public ActionResult UserAdd(string nid)
        {
            DegreeMapping.Models.User.Insert(nid);
            return RedirectToAction("ViewUsers");
        }

        public ActionResult UserDelete(string nid)
        {
            DegreeMapping.Models.User.Delete(nid);
            return RedirectToAction("ViewUsers");
        }

        public ActionResult UpdateUserRole(string nid, int roleId)
        {
            DegreeMapping.Models.User user = DegreeMapping.Models.User.Get(nid);
            DegreeMapping.Models.User.Update(nid, roleId, user.DisplayName);
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DisplayUserRoles(User user)
        {
            return PartialView(user);
        }

        #endregion

        #region Checklist

        public ActionResult Checklist() 
        {
            List<Degree> list_d = Degree.List(null,null);
            return View(list_d);
        }
        #endregion

        #region Colleges
        public ActionResult CollegeList()
        {
            List<College> list_c = College.List(null);
            return View(list_c);
        }

        public ActionResult _College(College c)
        {
            return PartialView(c);
        }

        public ActionResult CollegeSave(College c)
        {
            if (c.Id == 0)
            {
                College.Insert(c);
            }
            else
            {
                College.Update(c);
            }
            return RedirectToAction("CollegeList");
        }
        #endregion

        #region CourseMapper
        /// <summary>
        /// NOT USED, DELETE
        /// </summary>
        /// <param name="degreeId"></param>
        /// <returns></returns>
        public ActionResult _CodeMapper(int degreeId)
        {
            CourseMapper cm = new CourseMapper(degreeId);
            return PartialView(cm);
        }

        public ActionResult _CourseMapper(CourseMapper cm)
        {
            return PartialView(cm);
        }

        public ActionResult _CourseMapperHtml(CourseMapperHtml cmh)
        {
            return PartialView(cmh);
        }

        public ActionResult _CourseMapperList(int degreeId)
        {
            ViewBag.degreeId = degreeId;
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null, null);
            return PartialView(list_cm);
        }

        public ActionResult _CourseMapperListView(string title, List<Course> list_courses)
        {
            ViewBag.title = title;
            ViewBag.list_courses = list_courses;
            return PartialView();
        }

        public ActionResult CourseMapperEdit(int id)
        {
            CourseMapper cm = CourseMapper.Get(id);
            return View(cm);
        }

        public ActionResult CourseMapperAdd(int degreeId)
        {
            ViewBag.degreeId = degreeId;
            CourseMapper cm = new CourseMapper(degreeId);
            //List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
            return View(cm);
        }

        [HttpPost]
        public ActionResult CourseMapperSave(CourseMapper cm)
        {
            if (cm.Id > 0)
            {
                CourseMapper.Update(cm);
            }
            else 
            {
                CourseMapper.Insert(cm);
            }
            return RedirectToAction("DegreeView", new { id = cm.DegreeId });
        }

        public ActionResult CourseMapperDelete(int id, int degreeId)
        {
            CourseMapper.Delete(id);
            return RedirectToAction("DegreeView", new { id = degreeId });
        }

        #endregion

        #region Catalog
        public ActionResult CatalogView(int id)
        {
            DegreeMapping.Models.Catalog c = DegreeMapping.Models.Catalog.Get(id);
            return View(c);
        }

        /// <summary>
        /// Not sure if I still need this action/view...perhaps delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="degreeId"></param>
        /// <returns></returns>
        public ActionResult Catalog(int? id, int? degreeId)
        {
            List<DegreeMapping.Models.Catalog> list_cy = DegreeMapping.Models.Catalog.List();
            List<DegreeMapping.Models.Degree> list_d = new List<Degree>();
            if (id.HasValue)
            {
                list_d = DegreeMapping.Models.Degree.List(DegreeMapping.Models.Institution.UCFId,null).Where(x=>x.CatalogId == id.Value).ToList();
            }
            ViewBag.catagoryId = (id.HasValue) ? id.Value : 0;
            ViewBag.list_cy = list_cy;
            return View(list_d);
        }

        public ActionResult CatalogSave(Catalog cy)
        {
            if (cy.Id > 0)
            {
                DegreeMapping.Models.Catalog.Update(cy);
            }
            else
            {
                DegreeMapping.Models.Catalog.Insert(cy);
            }
            return RedirectToAction("Catalog");
        }
        #endregion

        #region NOT USED Cloning at Kamino
        public ActionResult CatalogClone()
        {
            return View();
        }

        public ActionResult CloneCourse(int sourceCatalogId, int targetCatalogId)
        {
            //Step 1: Clone Courses of source catalog
            Clone.CloneCourses(sourceCatalogId);
            //Step 2: Update DegreeId of source Catalog (this is done in the 1st stored procedure)
            //Clone.UpdateCourseDegreeId(sourceCatalogId, targetCatalogId);
            //Step 3: Update UCFCourseId
            Clone.UpdateUCFCourseId(targetCatalogId);

            return Json(new { status = 1, message = "this is the message" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CloneCourseMapping(int sourceCatalogId)
        {
            Clone.CloneCourseMapping(sourceCatalogId);
            return Json(new { status = 1, message = "this is the message" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CloneNotes(int sourceCatalogId)
        {
            Clone.CloneNote(sourceCatalogId);
            return Json(new { status = 1, message = "this is the message" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CloneCustomCourseSemester(int sourceCatalogId)
        {
            Clone.CloneCustomCourseSemester(sourceCatalogId);
            return Json(new { status = 1, message = "this is the message" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CloneCustomCourseMapper(int sourceCatalogId)
        {
            Clone.CloneCustomCourseMapper(sourceCatalogId);
            return Json(new { status = 1, message = "this is the message" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Clone Type 'Course','CourseMapper','Degree','Note'
        /// </summary>
        /// <param name="cloneId"></param>
        /// <param name="cloneType"></param>
        /// <returns></returns>
        public ActionResult _DisplayCloneInfo(int? cloneId, string cloneType)
        {
            ViewBag.CloneId = cloneId;
            ViewBag.CloneType = cloneType;
            return PartialView();
        }

        #endregion

        #region Custom Course Mapper
        public ActionResult _CustomCourseMapperView(int degreeId)
        {
            CustomCourseMapper ccm = CustomCourseMapper.Get(degreeId);
            return PartialView(ccm);
        }

        public ActionResult CustomCourseMapperEdit(int degreeId)
        {
            CustomCourseMapper ccm = CustomCourseMapper.Get(degreeId);
            return PartialView(ccm);
        }
        [HttpPost]
        public ActionResult CustomCourseMapperSave(int degreeId, bool hasRecord, List<int> courseId)
        {
            CustomCourseMapper ccm = new CustomCourseMapper(degreeId);
            ccm.HasRecord = hasRecord;
            ccm.List_CourseIds = courseId;
            if (ccm.HasRecord)
            {
                CustomCourseMapper.Update(ccm);
            }
            else
            {
                CustomCourseMapper.Insert(ccm);
            }
            CustomCourseMapper.Update(ccm);
            return RedirectToAction("CustomCourseMapperEdit", new { degreeId = ccm.DegreeId });
        }

        public ActionResult CourseMapperSortOrderUpdate(int courseMapperId, int sortOrderValue)
        {
            DegreeMapping.Models.CourseMapper.UpdateSortOrder(courseMapperId, sortOrderValue);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Custom Course Semester
        public ActionResult _CustomCourseSemsterView(int degreeId)
        {
            List<CustomCourseSemester> list_ccs = CustomCourseSemester.List(degreeId, null);
            ViewBag.DegreeId = degreeId;
            return PartialView(list_ccs);
        }
        
        public ActionResult _CustomCourseSemester(CustomCourseSemester ccs)
        {
            return PartialView(ccs);
        }

        public ActionResult CustomCourseSemesterAdd(int degreeId)
        {
            CustomCourseSemester ccs = new CustomCourseSemester(degreeId);
            return View(ccs);
        }

        public ActionResult CustomCourseSemesterEdit(int id)
        {
            CustomCourseSemester ccs = CustomCourseSemester.Get(id);
            return View(ccs);
        }

        public ActionResult CustomCourseSemesterDelete(int id, int degreeId)
        {
            CustomCourseSemester.Delete(id);
            return RedirectToAction("DegreeView", new { Id = degreeId });
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CustomCourseSemesterSave(CustomCourseSemester ccs)
        {
            if (ccs.Id == 0)
            {
                CustomCourseSemester.Insert(ccs);
            }
            else
            {
                CustomCourseSemester.Update(ccs);
            }
            return RedirectToAction("DegreeView", new { id = ccs.DegreeId });
        }




        #endregion

        #region TinyMCE
        public ActionResult _TinyMCE()
        {
            return PartialView();
        }
        #endregion

        #region Bread Crumbs
        public ActionResult _BreadCrumb(string startUrl, string institution, string degree)
        {
            return PartialView();
        }
        #endregion

        #region Degree Map Test
        //public ActionResult DegreeMapV2()
        //{
        //    DegreeMap dm = new DegreeMap();
        //    ViewBag.DegreeList = DegreeMapping.Models.Degree.List(null);
        //    return View(dm);
        //}
        public ActionResult DegreeMapV2(int? id)
        {
            DegreeMap dm = new DegreeMap();
            if (id.HasValue)
            {
                dm = DegreeMapping.Models.DegreeMap.Get(id.Value);
            }
            ViewBag.DegreeList = DegreeMapping.Models.Degree.List(null, null);
            return View(dm);
        }
        #endregion

        #region Favorite Degree
        public ActionResult _DisplayFavoriteDegree(string NID)
        {
            FavoriteDegree fd = FavoriteDegree.Get(NID);
            return PartialView(fd);
        }

        public ActionResult _DisplaySetFavoriteDegree(string NID, int degreeId)
        {
            FavoriteDegree fd = FavoriteDegree.Get(NID);
            ViewBag.DegreeId = degreeId;
            return PartialView(fd);
        }

        public ActionResult SetFavoriteDegree(string NID, int degreeId)
        {
            FavoriteDegree fd = FavoriteDegree.Get(NID);
            if (fd.DegreeList.Count == 0)
            {
                fd.DegreeList.Add(degreeId);
                FavoriteDegree.Insert(fd);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if (fd.DegreeList.Contains(degreeId))
            {
                fd.DegreeList.Remove(degreeId);
                FavoriteDegree.Update(fd);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                fd.DegreeList.Add(degreeId);
                FavoriteDegree.Update(fd);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region UpdateCourseMapperOrderBy
        public ActionResult UpdateCourseMapperOrderBy(int? id)
        {
            CourseMapperSort.SetAllSort();
            //CourseMapperSort.UpdateOrderBy(id);
            return View();
        }
        #endregion


        #region ErrrorLogging
        public ActionResult ErrorLog()
        {
            return View();
        }

        public ActionResult _DisplayFatalErrors()
        {
            List<ErrorLog> list_el = DegreeMapping.Models.ErrorLog.List("FATAL");
            return PartialView(list_el);
        }

        public ActionResult _DisplayErrors()
        {
            List<ErrorLog> list_el = DegreeMapping.Models.ErrorLog.List("ERROR");
            return PartialView(list_el);
        }

        public ActionResult _DisplayWarnErrors()
        {
            List<ErrorLog> list_el = DegreeMapping.Models.ErrorLog.List("WARN");
            return PartialView(list_el);
        }

        public ActionResult GetErrorException(int id)
        {
            string message = DegreeMapping.Models.ErrorLog.Get(id);
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}