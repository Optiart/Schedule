using Schedule.Models;
using Schedule.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Schedule.Controllers
{
    [RoutePrefix("api/Schedule")]
    public class ScheduleApiController : ApiController
    {
        private TabService _tabService;

        public ScheduleApiController()
        {
            _tabService = new TabService();
        }

        [Route("Tab/Save")]
        [HttpPost]
        public IHttpActionResult SaveScheduleForm(TabViewModel scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            _tabService.Save(scheduleModel);
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "success"));
        }

        [Route("Tab/Delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteTab([FromUri] int id)
        {
            _tabService.Delete(id);
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "success"));
        }
    }
}
