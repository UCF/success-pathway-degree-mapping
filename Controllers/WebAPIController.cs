using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DegreeMapping.Controllers
{
    [RoutePrefix("api/Courses")]
    //[MyCustomAttribute]
    public class WebAPIController : ApiController
    {
        [HttpGet]
        [Route("GetCourses")]
        public DegreeMap GetCourses()
        {
            DegreeMap dm = new DegreeMap(2,3);
            return dm;
        }
    }


}
//https://developerslogblog.wordpress.com/2016/12/30/adding-web-api-support-to-an-existing-asp-net-mvc-project/#Adding-WebAPI-support-to-our-MVC-Application