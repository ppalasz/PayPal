using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using PayPal.Business.BLL;

namespace PayPal.Web.Controllers
{
    public class FormController : Controller
    {
        private readonly X509Certificate2 _certificate ;
        private readonly string _thumb;

        public FormController()
        {
            //IConfiguration configuration = Functions.GetConfig();
            //_thumb = configuration["Thumb"];
            
            //_certificate = Security.GetLocalCertificate(_thumb);
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Find()
        {
            return View();
        }
        public ActionResult Update()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Auth()
        {
            string postedValues = "nic cert="+ _certificate.FriendlyName +" "+ _thumb.ToString();
            string response = "start ";

            ViewBag.certificate = _certificate;
            PostAuth postAuth=new PostAuth(){PostUrl = "https://localhost:445/api/Project", PostedValues = postedValues, Response = response };
            
            return View(postAuth);
        }

       
        [HttpPost]
        public ActionResult Auth(string postUrl, string methodSelect)
        {
            string postedValues = "end url=" + postUrl + " method=" + methodSelect;

            string response = "";
            using (var handler = new HttpClientHandler())
            {
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
                
                handler.ClientCertificates.Add(_certificate);

                //Ignoring SSL Certificate Errors On .NET Core On HttpClient
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                
                var myModel = new Dictionary<string, string>
                {
                    { "id","1" }
                };
                using (var content = new FormUrlEncodedContent(myModel))
                using (var client = new HttpClient(handler))
                {
                    HttpResponseMessage response2 = null;

                    switch (methodSelect.ToUpper())
                    {
                        case "GET":
                            response2=client.GetAsync(postUrl).Result;
                            break;
                        case "POST":
                            response2 = client.PostAsync(postUrl, content).Result;
                            break;
                        case "PUT":
                            response2 = client.PutAsync(postUrl, content).Result;
                            break;
                        case "DELETE":
                            response2 = client.DeleteAsync(postUrl).Result;
                            break;
                    }

                    if (response2 != null)
                    {
                        response2.EnsureSuccessStatusCode();
                        string jsonString = response2.Content.ReadAsStringAsync().Result;
                        response = jsonString.ToString();
                    }
                }


            }
            ViewBag.certificate = _certificate;
            PostAuth postAuth = new PostAuth() { PostUrl = postUrl, MethodSelect = methodSelect, PostedValues = postedValues, Response = response };
            return View(postAuth);
        }
    }
}