using Schedule.Models;
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
        public IHttpActionResult SaveScheduleForm(TabViewModel scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }


            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
