using Newtonsoft.Json;
using Schedule.DataAccess;
using Schedule.Domain.Models;
using Schedule.Models;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.Domain
{
    internal class ScheduleResultService : IScheduleResultService
    {
        private IResultRepository _resultRepository;

        public ScheduleResultService(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public void Process(Tab tab)
        {
            Result calculatedData = Calculate(tab);
            SaveResult(calculatedData, tab.Id);
        }

        private Result Calculate(Tab tab)
        {
            // Do stuff here
            return null;

            //return new Result
            //{
            //    Chain = new decimal[,] { { 1.2m, 5 }, { 2.2m, 4 }, { 3.2m, 8 }, { 4.2m, 8 }, { 5.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 }, { 6.2m, 5 } },
            //    AlgorithSummaries = new AlgorithSummary[]
            //        {
            //             new AlgorithSummary
            //             {
            //                Type = AlgorithmType.A1,
            //                Cstar = 4,
            //                Cmax = 17
            //             },
            //             new AlgorithSummary
            //             {
            //                Type = AlgorithmType.A2,
            //                Cstar = 7,
            //                Cmax = 97
            //             },
            //             new AlgorithSummary
            //             {
            //                Type = AlgorithmType.A3,
            //                Cstar = 6,
            //                Cmax = 777
            //             },
            //             new AlgorithSummary
            //             {
            //                Type = AlgorithmType.A4,
            //                Cstar = 66,
            //                Cmax = 76
            //             },
            //        },
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
            //    }, 
            //};
        }

        private void SaveResult(Result calculatedResult, int tabId)
        {
            var resultDto = new ResultDto
            {
                TabId = tabId,
                Chain = JsonConvert.SerializeObject(calculatedResult.Chain),
                Result = JsonConvert.SerializeObject(calculatedResult.PlotData)
            };

            IEnumerable<AlgorithmSummaryDto> summaryDto = ConvertSummaryToDto(calculatedResult.AlgorithSummaries);
            _resultRepository.Save(resultDto, summaryDto.ToArray());
        }

        private IEnumerable<AlgorithmSummaryDto> ConvertSummaryToDto(AlgorithSummary[] summaries)
        {
            foreach (var summary in summaries)
            {
                yield return new AlgorithmSummaryDto
                {
                    AlgorithmType = (byte)summary.Type,
                    Cstar = summary.Cstar,
                    Cmax = summary.Cmax
                };
            }
        }

        public Result GetResult(int tabId)
        {
            (ResultDto Result, AlgorithmSummaryDto[] Summaries) = _resultRepository.GetByTab(tabId);
            var result = new Result
            {
                Id = Result.Id,
                Chain = JsonConvert.DeserializeObject<decimal[,]>(Result.Chain),
                PlotData = JsonConvert.DeserializeObject<Dictionary<AlgorithmType, Plot>>(Result.Result)
            };

            result.AlgorithSummaries = new AlgorithSummary[Summaries.Length];
            int index = 0;
            foreach (var summary in Summaries)
            {
                result.AlgorithSummaries[index++] = new AlgorithSummary
                {
                    Type = (AlgorithmType)summary.AlgorithmType,
                    Cstar = summary.Cstar,
                    Cmax = summary.Cmax
                };
            }

            return result;
        }
    }
}