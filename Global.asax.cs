using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebApplication1.App_Start;

[assembly:log4net.Config.XmlConfigurator(Watch = true)]

namespace DegreeMapping
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

                if (ValidOrigin(HttpContext.Current.Request.Url.ToString()))
                {
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
                }
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }


        private static bool ValidOrigin(string url)
        {
            if (url.ToLower().Contains("connect.ucf.edu") || url.ToLower().Contains("smca.ucf.edu") || url.ToLower().Contains("portal.connect.ucf.edu"))
            {
                return true;
            }
            return false;
        }

        void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            HttpException httpException = ex as HttpException;
            int? httpStatusCode = (httpException != null ? httpException.GetHttpCode() : (int?)null);
            if (httpStatusCode.HasValue && httpStatusCode.Value >= 500)
            {
                log.Fatal(httpStatusCode.Value.ToString() + " Fatal Error", ex);
            }
            else if (httpStatusCode.HasValue && httpStatusCode.Value >= 400)
            { 
                log.Warn(httpStatusCode.Value.ToString() + " Warn Error", ex);
            } 
            else
            {
                log.Error(httpStatusCode.Value.ToString() + " Error", ex);
            }
        }

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new APIKeyMessageHandler());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();

        }
    }
}
