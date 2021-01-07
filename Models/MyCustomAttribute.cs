using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.UI.WebControls;

namespace DegreeMapping.Models
{
    public class MyCustomAttribute : ActionFilterAttribute
    {
        private int Id { get { return 3; } }
        private int Code { get { return 27; } }

        string parameter = "keyData";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments == null || !actionContext.ActionArguments.ContainsKey(parameter))
            {
                throw new Exception(string.Format("Parameter '{0}' not present ", parameter));
            }
            //ModelDataMethodResult 

            if (string.IsNullOrEmpty("Id") || string.IsNullOrEmpty("Code"))
            {
                throw new Exception("Error: could not find data");
            }

            base.OnActionExecuting(actionContext);
        }

    }
}
//https://developerslogblog.wordpress.com/2018/05/08/how-to-adding-custom-action-filters-to-a-web-api/