using Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Schedule.Controllers
{
    [RoutePrefix("api/Schedule")]
    public class ScheduleApiController : ApiController
    {
        [Route("Form/Save")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IHttpActionResult SaveScheduleForm(ScheduleViewModel scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }


            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
