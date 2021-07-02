using DegreeMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DegreeMapping.Controllers
{
    [RoutePrefix("api/Degree")]

    [EnableCors(origins: "https://connectucncmsqa.smca.ucf.edu", headers: "APIKey", methods: "*")]
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
        [MyCustomAttribute]
        [Route("GetDegreeMapV2")]
        public DegreeMap GetDegreeMapV2(int degreeId)
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
        #region APIv2
        [HttpGet]
        [Route("GetCatalogs")]
        public List<Catalog> GetCatalogs()
        {
            return Catalog.List().Where(x=>x.Active==true).OrderByDescending(x=>x.Current).ThenBy(x=>x.Year).ToList();
        }

        [HttpGet]
        [Route("GetActiveCatalogs")]
        public List<DegreeListv2> GetCurrentCatalog()
        {
            int currentCatalog = DegreeMapping.Models.Catalog.List().Where(x => x.Current).Select(x => x.Id).FirstOrDefault();
            return DegreeListv2.List(null, null, null, null, null);
        }

        [HttpGet]
        [Route("GetListByCatalog")]
        public List<DegreeListv2> GetListByCatalog(int catalogId)
        {
            return DegreeListv2.List(catalogId, null, null, null, null);
        }

        [HttpGet]
        [Route("GetListByCollege")]
        public List<DegreeListv2> GetListByCollege(int collegeId)
        {
            return DegreeListv2.List(null, collegeId, null, null, null);
        }

        [HttpGet]
        [Route("GetListByDegree")]
        public List<DegreeListv2> GetListByDegree(int degreeId)
        {
            return DegreeListv2.List(null, null, degreeId, null, null);
        }

        [HttpGet]
        [Route("GetListByInstitution")]
        public List<DegreeListv2> GetListByInstitution(int institutionId, int catalogId)
        {
            return DegreeListv2.List(catalogId, null, null, institutionId, null);
        }

        [HttpGet]
        [Route("GetListByUCFDegree")]
        public List<DegreeListv2> GetListByUCFDegree(int ucfDegreeId)
        {
            return DegreeListv2.List(null, null, null, null, ucfDegreeId);
        }








        //[HttpGet]
        //[Route("GetDegreeListv2")]
        //public List<DegreeListv2> GetDegreeListv2(int? catalogId, int? collegeId, int? degreeId, int? institutionId, int? ucfDegreeId)
        //{
        //    if (degreeId.HasValue)
        //    {
        //        return DegreeListv2.List(catalogId.Value, null, null, null, null);
        //    }
        //    else if (collegeId.HasValue)
        //    {
        //        return DegreeListv2.List(null, collegeId.Value, null, null, null);
        //    }
        //    else if (degreeId.HasValue)
        //    {
        //        return DegreeListv2.List(null, null, degreeId.Value, null, null);
        //    }
        //    else if (institutionId.HasValue)
        //    {
        //        return DegreeListv2.List(null, null, null, institutionId.Value, null);
        //    }
        //    else if (ucfDegreeId.HasValue)
        //    {
        //        return DegreeListv2.List(null, null, null, null, ucfDegreeId.Value);
        //    }
        //    else
        //    {
        //        int currentCatalog = DegreeMapping.Models.Catalog.List().Where(x => x.Current).Select(x => x.Id).FirstOrDefault();
        //        return DegreeListv2.List(currentCatalog, null, null, null,null);
        //    }
        //}

        [HttpGet]
        [Route("GetDegreeInfo")]
        public DegreeInfo GetDegreeInfo(int degreeId)
        {
            return DegreeInfo.Get(degreeId);
        }
        #endregion
    }


}
//https://developerslogblog.wordpress.com/2016/12/30/adding-web-api-support-to-an-existing-asp-net-mvc-project/#Adding-WebAPI-support-to-our-MVC-Application
//https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api

//https://stackoverflow.com/questions/5584923/a-cors-post-request-works-from-plain-javascript-but-why-not-with-jquery
//https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin