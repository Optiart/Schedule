using Newtonsoft.Json;
using Schedule.DataAccess;
using Schedule.Domain.Algorithms;
using Schedule.Domain.Models;
using Schedule.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

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
            var resultTable = new decimal?[4, 2];

            //------  input data  -------

            var jobPalette = tab.DurationByWork;
            var mashines = GenerateInputMashines(tab.DeviceProductivities);

            //--------  get chains  ----------

            List<List<int>> chains = ChainsAlgorithm.GetChains(jobPalette);

            //------  get jobsScheduling  -------

            decimal C_1;
            decimal? Cmax1;
            var jobsScheduling1 = ScheduleAlgorithm.BuildedSchedule(chains, mashines, AlgorithmType.A1, out C_1, out Cmax1);

            decimal C_2;
            decimal? Cmax2;
            var jobsScheduling2 = ScheduleAlgorithm.BuildedSchedule(chains, mashines, AlgorithmType.A2, out C_2, out Cmax2);

            decimal C_3;
            decimal? Cmax3;
            var jobsScheduling3 = ScheduleAlgorithm.BuildedSchedule(chains, mashines, AlgorithmType.A3, out C_3, out Cmax3);

            decimal C_4;
            decimal? Cmax4;
            var jobsScheduling4 = ScheduleAlgorithm.BuildedSchedule(chains, mashines, AlgorithmType.A4, out C_4, out Cmax4);

            //----- for result --------

            var ChainResult = new decimal[tab.DurationByWork.GetLength(0) * tab.DurationByWork.GetLength(1), 2];

            var index = 0;
            var chainNumber = 1;
            foreach (var job in chains)
            {
                decimal currentNum = chainNumber;

                foreach (var jobDuration in job)
                {
                    currentNum += .1m;

                    ChainResult[index, 0] = currentNum;
                    ChainResult[index, 1] = decimal.Round(jobDuration, 0);

                    index++;
                }

                chainNumber++;
            }

            var plot1 = new Plot();
            foreach (var val in jobsScheduling1)
            {
                plot1.Add(val.Key, val.Value);
            }

            var plot2 = new Plot();
            foreach (var val in jobsScheduling2)
            {
                plot2.Add(val.Key, val.Value);
            }

            var plot3 = new Plot();
            foreach (var val in jobsScheduling3)
            {
                plot3.Add(val.Key, val.Value);
            }

            var plot4 = new Plot();
            foreach (var val in jobsScheduling4)
            {
                plot4.Add(val.Key, val.Value);
            }

            return new Result
            {
                Chain = ChainResult,

                AlgorithSummaries = new AlgorithSummary[]
                {
                    new AlgorithSummary
                    {
                        Type = AlgorithmType.A1,
                        Cstar = decimal.Round(C_1,2),
                        Cmax = decimal.Round((decimal)Cmax1,2)
                    },
                    new AlgorithSummary
                    {
                        Type = AlgorithmType.A2,
                        Cstar = decimal.Round(C_2,2),
                        Cmax = decimal.Round((decimal)Cmax2,2)
                    },
                    new AlgorithSummary
                    {
                        Type = AlgorithmType.A3,
                        Cstar = decimal.Round(C_3,2),
                        Cmax = decimal.Round((decimal)Cmax3,2)
                    },
                    new AlgorithSummary
                    {
                        Type = AlgorithmType.A4,
                        Cstar = decimal.Round(C_4,2),
                        Cmax = decimal.Round((decimal)Cmax4,2)
                    },
                },

                PlotData = new Dictionary<AlgorithmType, Plot>
                {
                    {  AlgorithmType.A1,
                       plot1
                    },
                    {  AlgorithmType.A2,
                       plot2
                    },
                    {  AlgorithmType.A3,
                       plot3
                    },
                    {  AlgorithmType.A4,
                       plot4
                    },
                }
            };
        }

        public List<Mashine> GenerateInputMashines(decimal[] prod)
        {
            var mashines = new List<Mashine>();

            for (int i = 0; i < prod.Length; i++)
            {
                mashines.Insert(i, new Mashine
                {
                    Number = i + 1,
                    k = prod[i]
                });
            }
            //for (int i = 0; i < mashinesNumber; i++)
            //{
            //    var randomNumber1 = random1.Next(1,5);
            //    var randomNumber2 = random2.Next(0,9);

            //    string number = randomNumber1 + "," + randomNumber2;

            //    mashines.Insert(i, new Mashine
            //    {
            //        Number = i+1,
            //        k = Convert.ToDecimal(number)
            //    });
            //}

            return mashines;
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