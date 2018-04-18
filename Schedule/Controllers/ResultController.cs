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
        public ActionResult AvailableResults(int tabId = 1)
        {
            var fakeResult = new ResultViewModel
            {
                Id = 1,
                TabIds = new[] { 1, 2, 3 },
                Chain = new decimal[,] { { 1.2m, 5 }, { 2.2m, 4 }, { 3.2m, 8 }, { 4.2m, 8 }, { 5.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 } },
                GraphData = new Dictionary<AlgorithmType, Dictionary<int, DeviceGraphRow[]>>
                    {
                        { AlgorithmType.Algorithm1,  new Dictionary<int, DeviceGraphRow[]>
                            {
                                   { 6, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 19.1m,
                                            Start = 3,
                                            End = 7
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 20.1m,
                                            Start = 10,
                                            End = 15
                                        }
                                    }
                                },
                                { 5, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 19.1m,
                                            Start = 1,
                                            End = 4
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 20.1m,
                                            Start = 7,
                                            End = 9
                                        }
                                    }
                                }
                            }
                        },
                        { AlgorithmType.Algorithm2,  new Dictionary<int, DeviceGraphRow[]>
                            {
                                   { 1, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 2.1m,
                                            Start = 1,
                                            End = 3
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 1.1m,
                                            Start = 5,
                                            End = 18
                                        }
                                    }
                                },
                                { 2, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 5.1m,
                                            Start = 0,
                                            End = 2
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 6.1m,
                                            Start = 5,
                                            End = 9
                                        }
                                    }
                                }
                            }
                        },{ AlgorithmType.Algorithm3,  new Dictionary<int, DeviceGraphRow[]>
                            {
                                   { 3, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 3.1m,
                                            Start = 4,
                                            End = 8
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 6.1m,
                                            Start = 11,
                                            End = 19
                                        }
                                    }
                                },
                                { 4, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 5.1m,
                                            Start = 0,
                                            End = 2
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 6.1m,
                                            Start = 5,
                                            End = 9
                                        }
                                    }
                                }
                            }
                        },{ AlgorithmType.Algorithm4,  new Dictionary<int, DeviceGraphRow[]>
                            {
                                   { 8, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork =15.1m,
                                            Start = 0,
                                            End = 9
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 16.1m,
                                            Start = 11,
                                            End = 15
                                        }
                                    }
                                },
                                { 9, new DeviceGraphRow[]
                                    {
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 12.1m,
                                            Start = 2,
                                            End = 3
                                        },
                                        new DeviceGraphRow
                                        {
                                            PalleteWork = 13.1m,
                                            Start = 3,
                                            End = 6
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            return View(fakeResult);
        }
    }
}