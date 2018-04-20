using Schedule.Domain;
using Schedule.Domain.Models;
using Schedule.Models;
using System;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    [RoutePrefix("result")]
    public class ResultController : Controller
    {
        private IScheduleResultService _scheduleResultService;
        private ITabService _tabService;

        public ResultController(IScheduleResultService scheduleResultService, ITabService tabService)
        {
            _scheduleResultService = scheduleResultService;
            _tabService = tabService;
        }

        [Route("get")]
        public ActionResult AvailableResults(int? tabId = null)
        {
            int[] availableTabIds = _tabService.GetAllTabIds();

            if (availableTabIds == null || availableTabIds.Length == 0)
            {
                return View(model: null);
            }

            int targetTabId = tabId ?? availableTabIds[0];
            Result resultByTab;

            try
            {
                resultByTab = _scheduleResultService.GetResult(targetTabId);
            }
            catch
            {
                return View(model: null);
            }

            var resultViewModel = new ResultViewModel
            {
                Id = resultByTab.Id,
                Chain = resultByTab.Chain,
                AlgorithSummaries = resultByTab.AlgorithSummaries,
                PlotData = resultByTab.PlotData,
                TabIds = availableTabIds,
                CurrentTab = targetTabId
            };

            return View(resultViewModel);

            //var fakeResult = new ResultViewModel
            //{
            //    Id = 1,
            //    TabIds = new[] { 1, 2, 3 },
            //    Chain = new decimal[,] { { 1.2m, 5 }, { 2.2m, 4 }, { 3.2m, 8 }, { 4.2m, 8 }, { 5.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 } },
            //    AlgorithSummary = new AlgorithSummary[]
            //    {
            //         new AlgorithSummary
            //         {
            //            Type = AlgorithmType.A1,
            //            Cstar = 4,
            //            Cmax = 17
            //         },
            //         new AlgorithSummary
            //         {
            //            Type = AlgorithmType.A2,
            //            Cstar = 7,
            //            Cmax = 97
            //         },
            //         new AlgorithSummary
            //         {
            //            Type = AlgorithmType.A3,
            //            Cstar = 6,
            //            Cmax = 777
            //         },
            //         new AlgorithSummary
            //         {
            //            Type = AlgorithmType.A4,
            //            Cstar = 66,
            //            Cmax = 76
            //         },
            //    },
            //    PlotData = new Dictionary<AlgorithmType, Plot>
            //        {
            //            { AlgorithmType.A1, new Plot
            //                {
            //                       { 6, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 19.1m,
            //                                Duration = 4,
            //                                End = 7.7m
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 20.1m,
            //                                Duration = 5,
            //                                End = 15.7m
            //                            }
            //                        }
            //                    },
            //                    { 5, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 19.1m,
            //                                Duration = 3,
            //                                End = 4
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 20.1m,
            //                                Duration = 2,
            //                                End = 9
            //                            }
            //                        }
            //                    }
            //                }
            //            },
            //            { AlgorithmType.A2,  new Plot
            //                {
            //                       { 1, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 2.1m,
            //                                Duration = 2,
            //                                End = 3
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 1.1m,
            //                                Duration = 13,
            //                                End = 18
            //                            }
            //                        }
            //                    },
            //                    { 2, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 5.1m,
            //                                Duration = 2,
            //                                End = 2
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 6.1m,
            //                                Duration = 4,
            //                                End = 9
            //                            }
            //                        }
            //                    }
            //                }
            //            },{ AlgorithmType.A3,  new Plot
            //                {
            //                       { 3, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 3.1m,
            //                                Duration = 4,
            //                                End = 8
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 6.1m,
            //                                Duration = 7,
            //                                End = 18.5m
            //                            }
            //                        }
            //                    },
            //                    { 4, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 5.1m,
            //                                Duration = 2,
            //                                End = 2.3m
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 6.1m,
            //                                Duration = 4,
            //                                End = 9.7m
            //                            }
            //                        }
            //                    },
            //                    { 20, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 5.1m,
            //                                Duration = 2,
            //                                End = 2.3m
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 6.1m,
            //                                Duration = 4,
            //                                End = 9.7m
            //                            }
            //                        }
            //                    },
            //                    { 21, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 5.1m,
            //                                Duration = 2,
            //                                End = 2.3m
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 6.1m,
            //                                Duration = 4,
            //                                End = 9.7m
            //                            }
            //                        }
            //                    }
            //                }
            //            },{ AlgorithmType.A4,  new Plot
            //                {
            //                       { 8, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork =15.1m,
            //                                Duration = 9,
            //                                End = 9
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 16.1m,
            //                                Duration = 4,
            //                                End = 15
            //                            }
            //                        }
            //                    },
            //                    { 9, new PlotRowPerDevice[]
            //                        {
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 12.1m,
            //                                Duration = 1,
            //                                End = 3
            //                            },
            //                            new PlotRowPerDevice
            //                            {
            //                                PalletWork = 13.1m,
            //                                Duration = 3,
            //                                End = 6
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //};

            //return View(fakeResult);
        }
    }
}