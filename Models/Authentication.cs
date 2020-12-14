using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Configuration;
using System.Dynamic;

namespace DegreeMapping.Models
{
    public class Authentication
    {
        protected static string ServiceAcct { get { return "svc_RC_CEWWW"; } }

        protected static string ServiceAcctPass { get { return "Yg553@9tnb$Eh6eKTzzE"; } }

        public static string OU_SLAS { get { return "RC CE Web Admins"; } }

        public static int LoginTimeOut { get { return 60; } }

        public Authentication(string nid, string password, ref Models.User u)
        {
            using (var context = new PrincipalContext(ContextType.Domain, "NET", "NET\\" + ServiceAcct, ServiceAcctPass))
            {
                bool isValidAccount = context.ValidateCredentials(nid, password, ContextOptions.Negotiate);
                if (isValidAccount)
                {
                    UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(context, nid);
                    u.Authenticated = true;//System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                    u.DisplayName = oUserPrincipal.DisplayName;
                    u.Email = oUserPrincipal.EmailAddress;
                    u.NID = oUserPrincipal.Name;
                    u.FirstName = oUserPrincipal.GivenName;
                    u.LastName = oUserPrincipal.Surname;
                    //Check if person is in a specific OU
                    //PrincipalSearchResult<Principal> oPrincipalSearchResult = oUserPrincipal.GetGroups();
                    //foreach (Principal oResult in oPrincipalSearchResult)
                    //{
                    //    if (oResult.Name == OU_SLAS)
                    //    {
                    //        u.Authorized = Models.User.a;
                    //    }
                    //}
                }
                FormsAuthentication(u);
            }
        }
        private static void FormsAuthentication(User u)
        {
            string roles = GetRoles(u);

            System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket(
                1, //Ticket Version
                u.NID, //User Associated with ticket
                System.DateTime.Now, //DateTime Issued
                System.DateTime.Now.AddMinutes(LoginTimeOut), //DateTime to Expire
                false, //True for a persistent user cookie
                roles, //User-Data, in this case the 'Role'
                System.Web.Security.FormsAuthentication.FormsCookiePath //Cookie Path
                );

            string hash = System.Web.Security.FormsAuthentication.Encrypt(ticket); // Encrypt the cookie using the machine key for secure transport

            System.Web.HttpCookie cookie = new System.Web.HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, hash); //Name of Cookie and hashed Ticket

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration; // Set the cookie's expiration time to the tickets expiration time
            }
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);// Add the cookie to the list for outgoing response

            //System.Web.Security.FormsAuthentication.SetAuthCookie(u.NID, true);

        }

        public static string GetRoles(User u)
        {
            List<string> list_roles = new List<string>();
            list_roles.Add("authorized");
            if (u.NID == "jgiron" || u.NID == "mmalpica")
            {
                list_roles.Add("admin");
            }
            return string.Join(",", list_roles);
        }

    }
}