using Schedule.Models;
using Schedule.Domain;
using System.Linq;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("Schedule")]
    public class ScheduleController : Controller
    {
        private ITabService _tabService;

        public ScheduleController(ITabService tabService)
        {
            _tabService = tabService;
        }

        [Route("Available")]
        [HttpGet]
        public ActionResult AvailableSchedule()
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

        [Route("Result")]
        public ActionResult Result()
        {
            return View();
        }
    }
}