using Schedule.Models;
using Schedule.Services;
using System.Linq;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("Schedule")]
    public class ScheduleController : Controller
    {
        private TabService _tabService;

        public ScheduleController()
        {
            _tabService = new TabService();
        }

        [Route("Available")]
        [HttpGet]
        public ActionResult AvailableSchedule(int id = 1)
        {
            Tab[] tabs = _tabService.GetAll();
            TabViewModel[] tabsViewModel = tabs.Select(t => new TabViewModel(t)).ToArray();

            return View(tabsViewModel);
        }

        [Route("Form")]
        [HttpGet]
        public PartialViewResult ScheduleForm()
        {
            return PartialView("_ScheduleForm");
        }
    }
}