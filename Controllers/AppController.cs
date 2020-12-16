using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DegreeMapping.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;

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
            return RedirectToAction("InstitutionView", new { id=i.Id });
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
            return RedirectToAction("NoteView", "App", new { id = n.Id });
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

    }
}