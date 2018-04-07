using Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("Schedule")]
    public class ScheduleController : Controller
    {
        [Route("Available")]
        [HttpGet]
        public PartialViewResult AvailableSchedule()
        {
            return PartialView("_AvailableSchedule");
        }

        [Route("Form")]
        [HttpGet]
        public PartialViewResult ScheduleForm()
        {
            return PartialView("_ScheduleForm");
        }

        [Route("Form/Save")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveScheduleForm(ScheduleViewModel scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { message = "invalid" }, JsonRequestBehavior.AllowGet);
            }


            return null;
        }
    }
}