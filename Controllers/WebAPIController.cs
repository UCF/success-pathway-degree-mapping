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
    
    [EnableCors(origins: "*", headers:"*",methods:"*")]
    public class WebAPIController : ApiController
    {
        [HttpGet]
        [MyCustomAttribute]
        [Route("GetDegree")]
        public DegreeMap GetDegree(int institutionId, int degreeId)
        {
            DegreeMap dm = new DegreeMap(institutionId,degreeId);
            return dm;
        }

        [HttpGet]
        [Route("GetDegreeList")]
        public List<DegreeList> GetDegreeList()
        {
            return DegreeList.GetDegreeList();
        }
    }


}
//https://developerslogblog.wordpress.com/2016/12/30/adding-web-api-support-to-an-existing-asp-net-mvc-project/#Adding-WebAPI-support-to-our-MVC-Application
//https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api

//--->>>>>>https://stackoverflow.com/questions/5584923/a-cors-post-request-works-from-plain-javascript-but-why-not-with-jquery