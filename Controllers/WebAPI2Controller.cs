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
/// 
namespace DegreeMapping.Controllers
{
    [RoutePrefix("api/v2/DegreeMap")]

    [EnableCors(origins: 
        "https://connect.ucf.edu, " +
        "http://localhost:62752, " +
        "https://dev-ucf-ucn.pantheonsite.io, " +
        "https://test-ucf-ucn.pantheonsite.io, ",
        headers: "APIKey", methods: "*")]
    public class WebAPI2Controller : ApiController
    {
        /// <summary>
        /// Returns Success Pathway PDF Degree
        /// Helper Video: https://www.youtube.com/watch?v=jfk_-42rN9k
        /// </summary>
        /// <param name="degreeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPDFDegree")]
        [AllowAnonymous]
        public HttpResponseMessage GetPDFDegree(int degreeId)
        {
            PDFTemplate template = new PDFTemplate(degreeId);
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(template.HTMLPage);
            doc.DocumentInformation.Title = template.PDFTitle;
            doc.DocumentInformation.Subject = template.PDFSubject;
            doc.DocumentInformation.Author = template.PDFAuthor;
            doc.DocumentInformation.CreationDate = DateTime.Now;
            //doc.Save(@"C:\\temp\\DegreePathway_Download2.pdf");

            HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var stream = new MemoryStream(doc.Save());
            stream.Position = 0;

            //attachment
            //inline
            result.Content = new StreamContent(stream);

            //result.Content.Headers.Add("Content-Disposition:", "attachment; filename=Degree.pdf");
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = template.PDFFileName; //"SuccessPathwayDegree.pdf";
            //application/octet-stream
            //application/pdf
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentLength = stream.Length;

            doc.Close();
            return result;
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
