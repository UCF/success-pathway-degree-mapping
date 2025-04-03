using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Cors;
using DegreeMapper.PDFTemplate;
using DegreeMapperWebAPI;
using SelectPdf;

/// <summary>
/// This uses the DegreeMapperWebAPI Project
/// Not the Degree Mapping Models
/// </summary>
namespace DegreeMapping.Controllers
{
    [RoutePrefix("api/v2/DegreeMap")]

    [EnableCors(origins: 
        "https://connectucncmsqa.smca.ucf.edu, " +
        "https://connectucncmsdev.smca.ucf.edu, " +
        "https://connect.ucf.edu, " +
        "http://localhost:62752, " +
        "https://dev-ucf-ucn.pantheonsite.io, ",
        headers: "APIKey", methods: "*")]
    public class WebAPI2Controller : ApiController
    {
        [HttpGet]
        [Route("GetPDFDegree")]        
        public IHttpActionResult GetPDFDegree(int degreeId)
        {
            PDFTemplate template = new PDFTemplate(degreeId);
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(template.HTMLPage);
            doc.DocumentInformation.Title = template.PDFTitle;
            doc.DocumentInformation.Subject = template.PDFSubject;
            doc.DocumentInformation.Author = template.PDFAuthor;
            doc.DocumentInformation.CreationDate = DateTime.Now;

            //https://stackoverflow.com/questions/26038856/how-to-return-a-file-filecontentresult-in-asp-net-webapi
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            { 
                Content = new ByteArrayContent(doc.Save())
            };

            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = $"{template.PDFFileName}"
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var response = ResponseMessage(result);

            return response;

            //https://stackoverflow.com/questions/77603727/how-to-send-the-pdf-file-through-c-sharp-asp-net-web-api
            
        }


        [HttpGet]
        //[MyCustomAttribute]
        [Route("GetCatalogs")]
        public List<DegreeMapperWebAPI.Catalog> GetCatalogs()
        {
            return DegreeMapperWebAPI.Catalog.List().Where(x => x.Active && x.DisplayOnWeb).OrderByDescending(x => x.Year).ToList();
        }

        [HttpGet]
        [Route("GetListByCatalog")]
        public List<DegreeMapperWebAPI.DegreeList> GetListByCatalog(int catalogId)
        {
            return DegreeMapperWebAPI.DegreeList.List(catalogId, null).OrderBy(x => x.Degree).ThenBy(x => x.Institution).ToList();
        }

        [HttpGet]
        [Route("GetCourseMapper")]
        public List<CourseMapperJSON> GetCourseMapper(int degreeId)
        {
            return CourseMapperJSON.List(degreeId);
        }

        [HttpGet]
        [Route("GetDegreeInfo")]
        public DegreeInfo GetDegreeInfo(int degreeId)
        {
            return DegreeInfo.Get(degreeId);
        }

        [HttpGet]
        [Route("GetListByUCFDegree")]
        public List<DegreeList> GetListByUCFDegree(int ucfDegreeId)
        {
            return DegreeList.List(null, ucfDegreeId);
        }

        [HttpGet]
        [Route("GetCustomCourseMapper")]
        public List<CustomCourseMapper> GetCustomCourseMapper(int degreeId)
        {
            return CustomCourseMapper.List(degreeId).OrderBy(x => x.Semester).ThenBy(x => x.TermOrder).ToList();
        }

        [HttpGet]
        [Route("GetCustomCourseMapperHtml")]
        public string GetCustomCourseMapperHtml(int degreeId)
        {
            return string.Empty;
            //return CustomCourseMapper.List(degreeId);
        }

        [HttpGet]
        [Route("GetCustomCourseSemester")]
        public List<Models.CustomCourseSemester> GetCustomCourseSemester(int degreeId)
        {
            return Models.CustomCourseSemester.List(degreeId, null);
        }
    }
}
