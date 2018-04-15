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
                           { 6, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 19.1m,
                                    Duration = 3
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 20.1m,
                                    Duration = 5
                                }
                            }
                        },
                         { 5, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 15.1m,
                                    Duration = 3
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 16.1m,
                                    Duration = 5
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 17.1m,
                                    Duration = 1
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 18.1m,
                                    Duration = 2
                                }
                            }
                        },
                                                { 4, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 11.1m,
                                    Duration = 3
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 12.1m,
                                    Duration = 5
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 13.1m,
                                    Duration = 1
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 14.1m,
                                    Duration = 2
                                }
                            }
                        },
                        { 3, new DeviceGraphRow[]
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
                                    PalleteWork = 3.1m,
                                    Duration = 6
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 4.2m,
                                    Duration = 2
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 5.1m,
                                    Duration = 2
                                }
                            }
                        },
                        { 1, new DeviceGraphRow[]
                            {
                                new DeviceGraphRow
                                {
                                    PalleteWork = 6.1m,
                                    Duration = 2
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 7.1m,
                                    Duration = 2
                                },
                                new DeviceGraphRow
                                {
                                    PalleteWork = 8.2m,
                                    Duration = 2
                                }
                                ,
                                new DeviceGraphRow
                                {
                                    PalleteWork = 9.2m,
                                    Duration = 2
                                }
                                ,
                                new DeviceGraphRow
                                {
                                    PalleteWork = 10.2m,
                                    Duration = 2
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