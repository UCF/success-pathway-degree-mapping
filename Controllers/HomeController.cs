using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DegreeMapping.Models;
using System.ComponentModel;
//using System.Data.SqlTypes;
using System.DirectoryServices.ActiveDirectory;
using log4net;

namespace DegreeMapping.Controllers
{
    public class HomeController : Controller
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof("HomeController"));
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            //log4net.Config.XmlConfigurator.Configure();
            log.Debug("Test Debug error");
            log.Warn("Test Warn Error");
            log.Error("Test Error");
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Catalog", "App");
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
                    return RedirectToAction("Catalog", "App");
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

        [ActionName("Degree-Mapping")]
        public ActionResult DegreeMapping(int degreeId)
        {
            return View();
        }

        public ActionResult DegreeMapV2(int? degreeId)
        {
            return View();
        }

        public ActionResult _DegreeMapV2WPTemplate()
        {
            return PartialView();
        }

        public ActionResult DegreeMapV3(int? degreeId)
        {
            return View();
        }

        public ActionResult _DegreeMapV3WPTemplate()
        {
            return PartialView();
        }

        public ActionResult _DegreeMapV3Footer()
        {
            return PartialView();
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