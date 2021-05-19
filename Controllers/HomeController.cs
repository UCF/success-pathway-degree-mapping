using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DegreeMapping.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Institutions", "App");
            }
            Models.User u = new Models.User();
            return View(u);
        }

        [HttpPost]
        public ActionResult Index(User u)
        {
            if (!string.IsNullOrEmpty(u.NID)) 
            {
                Models.User user = new User(u.NID);
                new Authentication(u.NID, u.Password, ref user);
                if (user.Authenticated && user.Authorized)
                {
                    return RedirectToAction("Institutions", "App");
                }
                else 
                {
                    u = user;
                    if (!u.Authorized) { u.Message += "Account not authorized"; }
                    if (!u.Authenticated) { u.Message += "Account not authenticated"; }
                }
            }
            return View(u);
        }

        public ActionResult SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        public ActionResult Example()
        {
            return View();
        }

        public ActionResult DegreeList()
        {
            return View();
        }

        public ActionResult DegreeGeneric()
        {
            return View();
        }

        public ActionResult DegreeMap()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Decisiontree()
        {
            return View();
        }
    }
}