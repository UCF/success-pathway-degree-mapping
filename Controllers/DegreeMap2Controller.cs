using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DegreeMapping.Controllers
{
    public class DegreeMap2Controller : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PathwayDegreeList()
        {
            return View();
        }

        public ActionResult _PathwayDegreeListTemplate()
        {
            return PartialView();
        }

        public ActionResult _PathwayDegreeFooter()
        {
            return PartialView();
        }

        public ActionResult Catalog2020_2021()
        {
            return View();
        }

        public ActionResult Catalog2021_2022()
        {
            return View();
        }

        public ActionResult DegreeMap()
        {
            return View();
        }
        
    }
}
