using Schedule.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Schedule.Domain.Algorithms
{
    public class Job
    {
        public string Number { get; set; }

        public int p { get; set; }

        public decimal? C { get; set; }
    }
    public class Mashine
    {
        public int Number { get; set; }

        public decimal k { get; set; }

        public decimal f { get; set; }

        public decimal T { get; set; }
    }

    public static class ScheduleAlgorithm
    {
        static List<List<Job>> _jobsSchedulingList;

        public static Dictionary<int, PlotRowPerDevice[]> BuildedSchedule(
            List<List<int>> chains, 
            List<Mashine> mashin,
            AlgorithmType algorythmType, 
            out decimal C_, 
            out decimal? Cmax)
        {
            _jobsSchedulingList = new List<List<Job>>();

            //--------  initialize job, mashine and jobsSchedulingList   ----------

            var jobChains = new List<List<Job>>();

            for (int i = 0; i < chains.Count; i++)
            {
                var newList = new List<Job>();

                for (int j = 0; j < chains[i].Count; j++)
                {
                    var job = new Job
                    {
                        Number = i + 1 + "," + (j + 1),
                        p = chains[i][j],
                        C = null
                    };

                    newList.Add(job);
                }

                jobChains.Insert(i, newList);
            }

            var mashines = new List<Mashine>();
            for (int i = 0; i < mashin.Count; i++)
            {
                var newMashine = new Mashine
                {
                    Number = i + 1,
                    k = mashin[i].k
                };

                mashines.Add(newMashine);
            }

            for (int i = 0; i < mashines.Count; i++)
            {
                _jobsSchedulingList.Insert(i, new List<Job>());
            }

            //--------  START ALGORYTHM  ----------

            C_ = DoAlgorythm(jobChains, mashines, algorythmType);

            var maxList = _jobsSchedulingList.Select(x => x.OrderByDescending(y => y.C).Select(y => y.C).FirstOrDefault()).ToList();
            Cmax = maxList.Max();

            //-------- create Dictionary result ----------

            return GetDictionaryJobsScheduling(mashines);            
        }

        private static Dictionary<int, PlotRowPerDevice[]> GetDictionaryJobsScheduling(List<Mashine> mashines)
        {
            var _jobsScheduling = new Dictionary<int, PlotRowPerDevice[]>();

            int count = 1;
            foreach(var mashineList in _jobsSchedulingList)
            {
                PlotRowPerDevice[] jobList = new PlotRowPerDevice[mashineList.Count];

                for(int i=0; i< mashineList.Count; i++ )
                {
                    var job = new PlotRowPerDevice
                    {
                        PalletWork = Convert.ToDecimal(mashineList[i].Number),
                        Duration = mashineList[i].p * mashines[count - 1].k,
                        End = (decimal)mashineList[i].C
                    };

                    jobList[i] = job;
                }

                _jobsScheduling.Add(count, jobList);
                count++;
            }

            return _jobsScheduling;
        }

        public static decimal DoAlgorythm(List<List<Job>> jobChains, List<Mashine> mashines, AlgorithmType algorythmType)
        {
            bool finish = false;

            //------ initial value: found C* and f  ----------

            var C_ = GetC_(jobChains, mashines);

            GetMashinesKoeficient(ref mashines, C_);

            List<Job> jobsForAddToScheduling = JobForAddToJobsScheduling(jobChains);

            while (!finish)
            {
                //---------  found job and mashine   ---------

                var jobMAX = jobsForAddToScheduling.OrderByDescending(x => x.p).Select(x => x).FirstOrDefault();

                List<Mashine> allowMashines = FoundAllowMashines(mashines, jobChains, jobMAX, algorythmType);

                var mashineTru = GetTrueMashine(allowMashines, algorythmType);

                //---------  add job to mashine   ---------

                _jobsSchedulingList[mashineTru.Number - 1].Add(jobMAX);

                //---------  refreshe data   ---------

                jobsForAddToScheduling.Remove(jobMAX);

                RefresheKoef(jobChains, mashineTru, jobMAX, algorythmType);

                //----------  finish check -----------

                string[] jobNumber = jobMAX.Number.Split(',');

                if (jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count > Convert.ToInt32(jobNumber[1]))
                {
                    jobsForAddToScheduling.Add(jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])]);
                }

                if (jobsForAddToScheduling.Count == 0)
                {
                    finish = true;
                }
            }

            return C_;
        }

        private static void RefresheKoef(List<List<Job>> jobChains, Mashine mashineTru, Job jobMAX, AlgorithmType algorythmType)
        {
            string[] jobNumber = jobMAX.Number.Split(',');

            if (algorythmType == AlgorithmType.A1 || algorythmType == AlgorithmType.A3)
            {
                mashineTru.T += mashineTru.k * jobMAX.p;
                mashineTru.f -= jobMAX.p;
            }
            else
            {
                decimal? lastJobC = 0;

                if (jobNumber[1] != "1")
                {
                    lastJobC = jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2].C;
                }

                if (mashineTru.T >= lastJobC)
                {
                    mashineTru.T += mashineTru.k * jobMAX.p;
                    mashineTru.f -= jobMAX.p;
                }
                else
                {
                    var current_T = (decimal)(lastJobC + mashineTru.k * jobMAX.p);
                    mashineTru.f -= (current_T - mashineTru.T) / mashineTru.k;
                    mashineTru.T = current_T;                    
                }
            }

            jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 1].C = mashineTru.T;
        }

        private static Mashine GetTrueMashine(List<Mashine> allowMashines, AlgorithmType algorythmType)
        {
            var trueMashine = new Mashine();

            if (algorythmType == AlgorithmType.A1 || algorythmType == AlgorithmType.A2)
            {
                trueMashine = allowMashines.OrderByDescending(x => x.f).Select(x => x).FirstOrDefault();
            }
            else
            {
                trueMashine = allowMashines[GetRandomNumber(0, (byte)allowMashines.Count)];
            }

            return trueMashine;
        }

        private static List<Mashine> FoundAllowMashines(List<Mashine> mashines, List<List<Job>> jobChains, Job jobMAX, AlgorithmType algorythmType)
        {
            var allowMashines = new List<Mashine>();

            string[] jobNumber = jobMAX.Number.Split(',');

            if (algorythmType == AlgorithmType.A1 || algorythmType == AlgorithmType.A3)
            {
                if (jobNumber[1] != "1")
                {
                    foreach (var mashine in mashines)
                    {
                        var lastJob_C = (decimal)jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2].C;

                        if (mashine.T >= lastJob_C)
                        {
                            allowMashines.Add(mashine);
                        }
                    }
                }
                else
                {
                    allowMashines = mashines;
                }
            }
            else
            {
                allowMashines = mashines;
            }

            return allowMashines;
        }

        private static List<Job> JobForAddToJobsScheduling(List<List<Job>> jobChains)
        {
            List<Job> jobsForScheduling = new List<Job>();

            for (int i = 0; i < jobChains.Count; i++)
            {
                jobsForScheduling.Add(jobChains[i][0]);
            }

            return jobsForScheduling;
        }

        private static void GetMashinesKoeficient(ref List<Mashine> mashines, decimal C_)
        {
            foreach (var mashine in mashines)
            {
                mashine.f = C_ / mashine.k;
                mashine.T = 0;
            }
        }

        private static decimal GetC_(List<List<Job>> jobChains, List<Mashine> mashines)
        {
            var mashinesOrder = mashines.OrderBy(x => x.k).Select(x => x.k).ToList();

            var jobChainsOrder = jobChains.OrderByDescending(x => x.Select(y => y.p).Sum()).Select(x => x.Select(e => e.p).Sum()).ToList();

            var С_List = new decimal[mashines.Count + 1];

            for (int i = 0; i < mashines.Count; i++)
            {
                С_List[i] = mashinesOrder[i] * jobChainsOrder[i];
            }

            foreach (var jobChain in jobChains)
            {
                foreach (var job in jobChain)
                {
                    decimal tmp = 0;

                    foreach (var mashine in mashines)
                    {
                        tmp += (1 / (mashine.k * job.p));
                    }

                    С_List[mashines.Count] += (1 / tmp);
                }
            }

            var C_ = С_List.Max();

            return C_;
        }

        static byte GetRandomNumber(byte minValue, byte maxValue)
        {
            byte[] bytes = new byte[1];
            using (var randomProvider = new RNGCryptoServiceProvider())
            {
                randomProvider.GetBytes(bytes);
            }

            return (byte)((bytes[0] % (maxValue - minValue)) + minValue);
        }
    }
}
