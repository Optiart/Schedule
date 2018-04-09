using Schedule.Models;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("Schedule")]
    public class ScheduleController : Controller
    {
        [Route("Available/Tabs")]
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult AvailableScheduleTabs()
        {
            int[] ids = new int[1] { 1 };
            return PartialView("_AvailableScheduleTabs", ids);
        }

        [Route("Available")]
        [HttpGet]
        public PartialViewResult AvailableSchedule(int id = 1)
        {
            var model = new TabViewModel
            {
                DeviceType = Models.Enums.DeviceType.Identical,
                NumberOfWorkPerRow = 1,
                NumberOfDevices = 2,
                NumberOfPalleteRows = 3
            };

            return PartialView("_AvailableSchedule", model);
        }

        [Route("Form")]
        [HttpGet]
        public PartialViewResult ScheduleForm()
        {
            return PartialView("_ScheduleForm");
        }
    }
}