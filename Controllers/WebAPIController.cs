using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DegreeMapping.Controllers
{
    [RoutePrefix("api/Degree")]
    
    [EnableCors(origins: "https://connectucncmsqa.smca.ucf.edu", headers: "APIKey", methods:"*")]
    public class WebAPIController : ApiController
    {
        [HttpGet]
        [MyCustomAttribute]
        [Route("GetDegreeMap")]
        public DegreeMap GetDegreeMap(int degreeId)
        {
            DegreeMap dm = DegreeMap.Get(degreeId);
            return dm;
        }

        [HttpGet]
        [Route("GetGenericList")]
        public List<Generic> GetGenericList()
        {
            return Generic.List();
        }


        [HttpGet]
        [Route("GetDegreeList")]
        public List<DegreeList> GetDegreeList()
        {
            return DegreeList.List();
        }
    }


}
//https://developerslogblog.wordpress.com/2016/12/30/adding-web-api-support-to-an-existing-asp-net-mvc-project/#Adding-WebAPI-support-to-our-MVC-Application
//https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api

//https://stackoverflow.com/questions/5584923/a-cors-post-request-works-from-plain-javascript-but-why-not-with-jquery
//https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin