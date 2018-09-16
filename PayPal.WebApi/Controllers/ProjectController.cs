using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PayPal.Business.BLL;
using PayPal.Business.DAL.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using PayPal.Business.DAL;

namespace PayPal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class ProjectController : Controller
    {
        //private readonly X509Certificate2 _certificate;

        private IProjectRepository Repository { get; set; }

        public ProjectController(IProjectRepository repository)
        {
            this.Repository = repository;
        }

       


        [HttpGet]
        public JsonResult GetVendorsForProject(int id=0)
        {
            ProjectLib projectLib = new ProjectLib(Repository);
            if (id == 0)
            {
                var projects = projectLib.GetProjects();
                Response.StatusCode = 200;
                return Json(projects);
            }
            else
            { 
                var project = projectLib.FindProject(id);
                Vendors vendors = projectLib.GetVendors(id);
                Response.StatusCode = 200;
                return Json(project);
            }

            
        }

        [HttpPut]
        public JsonResult Update([FromBody] ProjectData p)
        {
            //X509Certificate2 clientCertificate = Security.GetClientCertificate(HttpContext);
            ProjectLib projectLib = new ProjectLib(Repository);
            projectLib.UpdateProject(p);

            return Json("OK");
        }
        
        [HttpPost]
        public JsonResult Add([FromBody] ProjectData p)
        {
            //X509Certificate2 clientCertificate = Security.GetClientCertificate(HttpContext);
            ProjectLib projectLib = new ProjectLib(Repository);
            VwProject p1=projectLib.AddNewProject(p);
           
            return Json(p1);
        }
    }
}
