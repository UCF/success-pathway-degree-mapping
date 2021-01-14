using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.App_Start;


namespace DegreeMapping
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            // Preflight request comes with HttpMethod OPTIONS
            // The following line solves the error message
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET");
                // If any http headers are shown in preflight error in browser console add them below
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Pragma, Cache-Control, Authorization, APIKey");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin","*");
                HttpContext.Current.Response.End();
            }
        }
        //protected void Application_BeginRequest()
        //{


        //    if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
        //    {
        //        Response.Flush();
        //    }
        //}

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new APIKeyMessageHandler());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
