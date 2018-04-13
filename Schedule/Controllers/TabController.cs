using Schedule.Models;
using Schedule.Domain;
using System.Linq;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("tab")]
    public class TabController : Controller
    {
        private ITabService _tabService;

        public TabController(ITabService tabService)
        {
            _tabService = tabService;
        }

        [Route("available")]
        public ActionResult AvailableTabs()
        {
            Tab[] tabs = _tabService.GetAll();
            TabViewModel[] tabsViewModel = tabs.Select(t => new TabViewModel(t)).ToArray();

            return View(tabsViewModel);
        }

        [Route("add")]
        public ActionResult AddTab()
        {
            return View();
        }
    }
}