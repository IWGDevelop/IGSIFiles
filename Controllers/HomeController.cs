using System.Web.Mvc;

namespace Iwg.NOMBRE_DE_LA_EMPRESA.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}