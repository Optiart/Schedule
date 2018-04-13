using System.Web.Mvc;

namespace Schedule.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("home/index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}