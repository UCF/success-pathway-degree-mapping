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
        protected static string ServiceAcct { get { return "svc_ucn_Degreemap"; } }

        protected static string ServiceAcctPass { get { return "l#j5I)XXGDY951D1m!b2rc7@Q"; } }

        public static string OU_SLAS { get { return "RC CE Web Admins"; } }

        public static int LoginTimeOut { get { return 60; } }

        /// <summary>
        /// Setting up Authentication
        /// https://stackoverflow.com/questions/36538958/if-formsauthentication-ticket-is-set-why-doesnt-user-isinrole-admin-work
        /// </summary>
        /// <param name="nid"></param>
        /// <param name="password"></param>
        /// <param name="u"></param>
        public Authentication(string nid, string password, ref Models.User u)
        {
            //SetFakeAuthentication(ref u);
            //return;

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

        public void SetFakeAuthentication(ref Models.User u)
        {
                u.Authenticated = true;//System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                u.DisplayName = "Joseph";
                u.Email = "Joseph.Giron@ucf.edu";
                u.NID = "jgiron";
                u.FirstName = "Joseph";
                u.LastName = "Giron";
                FormsAuthentication(u);
        }


        private static void FormsAuthentication(User u)
        {
            string roles = GetRoles(u.RoleId);
            string name = (!string.IsNullOrEmpty(u.DisplayName)) ? u.DisplayName : u.NID;
            System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket(
                1, //Ticket Version
                name, //User Associated with ticket
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
            Models.User.Update(u.NID, u.RoleId, u.DisplayName);
        }

        public static string GetRoles(int roleId)
        {
            List<string> myRoles = new List<string>();
            switch (roleId)
            {
                case 1:
                    myRoles.Add(DegreeMapping.Models.Role.SuperAdmin);
                    myRoles.Add(DegreeMapping.Models.Role.Admin);
                    myRoles.Add(DegreeMapping.Models.Role.Publisher);
                    myRoles.Add(DegreeMapping.Models.Role.Editor);
                    break;
                case 2:
                    myRoles.Add(DegreeMapping.Models.Role.Admin);
                    myRoles.Add(DegreeMapping.Models.Role.Publisher);
                    myRoles.Add(DegreeMapping.Models.Role.Editor);
                    break;
                case 3:
                    myRoles.Add(DegreeMapping.Models.Role.Publisher);
                    myRoles.Add(DegreeMapping.Models.Role.Editor);
                    break;
                default:
                    myRoles.Add(DegreeMapping.Models.Role.Editor);
                    break;
            }
            return string.Join(",", myRoles);
        }
    }
}