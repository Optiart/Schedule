using Schedule.Domain;
using Schedule.Domain.Models;
using Schedule.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("result")]
    public class ResultController : Controller
    {
        private IScheduleResultService _scheduleResultService;

        public ResultController(IScheduleResultService scheduleResultService)
        {
            _scheduleResultService = scheduleResultService;
        }

        [Route("all")]
        public ActionResult AvailableResults()
        {
            var fakeResults = new ResultViewModel[]
            {
                new ResultViewModel
                {
                    Id = 1,
                    Chain = new decimal[2,2] { { 1.2m, 5 }, { 1.2m, 7 } },
                    GraphData = new Dictionary<int, DeviceGraphRow[]>
                    {
                        { 1, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 1.1m,
                                    Duration = 3
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 2.1m,
                                    Duration = 5
                                }
                            }
                        },
                        { 2, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 2.1m,
                                    Duration = 6
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 1.2m,
                                    Duration = 2
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 3.1m,
                                    Duration = 2
                                }
                            }
                        },
                        { 3, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 4.1m,
                                    Duration = 9
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 5.1m,
                                    Duration = 7
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 5.2m,
                                    Duration = 12
                                }
                            }
                        }
                    }
                }
            };

            return View(fakeResults);
        }
    }
}