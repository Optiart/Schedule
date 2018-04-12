using Schedule.Models;
using Schedule.Domain;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Schedule.Controllers
{
    [RoutePrefix("api/Tab")]
    public class TabApiController : ApiController
    {
        private ITabService _tabService;

        public TabApiController(ITabService tabService)
        {
            _tabService = tabService;
        }

        [Route("Save")]
        [HttpPost]
        public IHttpActionResult SaveScheduleForm(TabViewModel saveTabRequest)
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

            _tabService.Save(tab);
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "success"));
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteTab([FromUri] int id)
        {
            _tabService.Delete(id);
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "success"));
        }
    }
}
