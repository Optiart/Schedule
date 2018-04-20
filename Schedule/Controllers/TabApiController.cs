using Schedule.Models;
using Schedule.Domain;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Schedule.Controllers
{
    [RoutePrefix("api/tab")]
    public class TabApiController : ApiController
    {
        private ITabService _tabService;
        private IScheduleResultService _scheduleResultService;

        public TabApiController(ITabService tabService, IScheduleResultService scheduleResultService)
        {
            _tabService = tabService;
            _scheduleResultService = scheduleResultService;
        }

        [Route("save", Name = "SaveTabRoute")]
        [HttpPost]
        public IHttpActionResult Save(TabViewModel saveTabRequest)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            var tab = new Tab
            {
                DeviceProductivities = saveTabRequest.DeviceProductivities,
                DeviceType = saveTabRequest.DeviceType,
                DurationByWork = saveTabRequest.DurationByWork,
                NumberOfDevices = saveTabRequest.NumberOfDevices,
                NumberOfPalleteRows = saveTabRequest.NumberOfPalleteRows,
                NumberOfWorkPerRow = saveTabRequest.NumberOfWorkPerRow
            };

            tab.Id = _tabService.Save(tab);
            _scheduleResultService.Process(tab);

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "success"));
        }

        [Route("delete/{id}", Name = "DeleteTabRoute")]
        [HttpDelete]
        public IHttpActionResult Delete([FromUri] int id)
        {
            _tabService.Delete(id);
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "success"));
        }
    }
}
