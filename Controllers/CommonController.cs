using Iwg.Entities.Transverse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Iwg.NOMBRE_DE_LA_EMPRESA.Presentation.Controllers
{
    public class CommonController : Controller
    {
        static readonly HttpClient clientHTTP = new HttpClient();
        private readonly string urlApiTransverse = string.Empty;

        public CommonController()
        {
            urlApiTransverse = ConfigurationManager.AppSettings["apiIWGTransverse"].ToString();
        }

        // GET: Common
        public async Task<ActionResult> GetMenus(string company)
        {
            var userString = HttpContext.GetOwinContext().Authentication.User.Claims.First(x => x.Type == ClaimTypes.Upn).Value;

            string urlServices = WebConfigurationManager.AppSettings["apiIWGTransverse"];
            string uri = urlServices + "/menu/GetMenubyUserCompany/" + userString.Split('@')[0] + "/"+company;
            using (HttpClient httpClient = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                Task<string> response = httpClient.GetStringAsync(uri);
                var list = JsonConvert.DeserializeObjectAsync<List<ItemMenu>>(response.Result).Result;
                return PartialView(list);
            }
        }

        [HttpPost]
        public ActionResult ExcelSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        public string GetUrlApi()
        {
            string urlServices = string.Empty;
            try
            {
                urlServices = ConfigurationManager.AppSettings["apiIWGTransverse"];
            }
            catch (Exception)
            {
                throw new ArgumentException("Error al consultar la configuración del servicio", new Exception("Error al consultar la configuración del servicio"));
            }

            return urlServices;
        }

        public string GetUrlTransverse()
        {
            string urlServices = string.Empty;
            try
            {
                urlServices = ConfigurationManager.AppSettings["urlIWGTransverse"];
            }
            catch (Exception)
            {
                throw new ArgumentException(message: "Error al consultar la configuración del servicio", innerException: new Exception(message: "Error al consultar la configuración del servicio"));
            }

            return urlServices;
        }
    }
}