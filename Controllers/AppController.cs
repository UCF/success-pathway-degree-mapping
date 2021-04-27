using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DegreeMapping.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;
using Microsoft.Win32;

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

        public ActionResult _Dashboard()
        {
            return PartialView();
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
        public ActionResult DegreeAdd(int institutionId)
        {
            Degree d = new Degree(institutionId);
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
        #endregion

        #region Code - Updates course code only
        public ActionResult CodeList()
        {
            List<Course> list_c = Course.List(null);
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
            
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
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
            list_cm = CourseMapper.List(degreeId, null);
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
            List<string> list_users = DegreeMapping.Models.User.List();
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
        #endregion

        #region Checklist

        public ActionResult Checklist() 
        {
            List<Degree> list_d = Degree.List(null);
            return View(list_d);
        }
        #endregion

        #region Issues
        public ActionResult Issues()
        {
            List<Issue> list_i = Issue.List(null);
            return View();
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
        public ActionResult _CodeMapper(int degreeId)
        {
            CourseMapper cm = new CourseMapper(degreeId);
            return PartialView(cm);
        }

        public ActionResult _CourseMapper(CourseMapper cm)
        {
            return PartialView(cm);
        }

        public ActionResult _CourseMapperList(int degreeId)
        {
            List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
            return PartialView(list_cm);
        }

        public ActionResult CourseMapperEdit(int id)
        {
            CourseMapper cm = CourseMapper.Get(id);
            return View(cm);
        }

        public ActionResult CourseMapperAdd(int degreeId)
        {
            CourseMapper cm = new CourseMapper(degreeId);
            //List<CourseMapper> list_cm = CourseMapper.List(degreeId, null);
            return View(cm);
        }

        public ActionResult CourseMapperUpdateOrderby(int id, int orderby, int degreeId)
        {
            CourseMapper.UpdateCourseMapperOrderby(id, orderby);
            return RedirectToAction("DegreeView", new { id = degreeId });
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
                List<CourseMapper> list_cm = CourseMapper.List(cm.DegreeId, null);
                int orderby = list_cm.Max(x => x.OrderBy)+1;
                cm.OrderBy = orderby;
                int id = CourseMapper.Insert(cm);
                cm.Id = id;
            }
            return RedirectToAction("DegreeView", new { id = cm.DegreeId } );
        }

        public ActionResult CourseMapperDelete(int id, int degreeId)
        {
            CourseMapper.Delete(id);
            return RedirectToAction("DegreeView", new { id = degreeId });
        }

        #endregion

    }
}