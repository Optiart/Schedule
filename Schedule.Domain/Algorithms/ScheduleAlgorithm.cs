using Schedule.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Schedule.Domain.Algorithms
{
    public class Job : ICloneable
    {
        public string Number { get; set; }

        public int p { get; set; }

        public decimal? C { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
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

            if(C_ - decimal.Round(C_,0) > 0)
            {
                C_ = decimal.Round(C_, 0) + 1;
            }
            else
            {
                decimal.Round(C_, 0);
            }

            var maxList = _jobsSchedulingList.Select(x => x.OrderByDescending(y => y.C).Select(y => y.C).FirstOrDefault()).ToList();
            Cmax = maxList.Max();

            //-------- create Dictionary result ----------


            var maxLedgeMashineNumber = FoundMaxLedge();
            bool impruve;

            do
            {
                impruve = ImpruveSchedule(maxLedgeMashineNumber, jobChains, mashines);

                maxLedgeMashineNumber = FoundMaxLedge();
                var mashineList = _jobsSchedulingList[maxLedgeMashineNumber];
                Cmax = mashineList[mashineList.Count - 1].C;

                if (Cmax == C_)
                {
                    break;
                }

            } while (impruve);



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
                        PalletWork = Convert.ToDecimal(mashineList[i].Number.Replace(',','.')),
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


        private static int FoundMaxLedge()
        {
            var mashineNumber = 0;
            decimal? maxC = 0;

            var index = 0;
            foreach (var mashineList in _jobsSchedulingList)
            {
                if(mashineList.Count != 0)
                {
                    if (maxC < mashineList[mashineList.Count - 1].C)
                    {
                        maxC = mashineList[mashineList.Count - 1].C;
                        mashineNumber = index;
                    }
                }                    

                index++;
            }

            return mashineNumber;
        }


        private static bool ImpruveSchedule(int maxLedgeMashineNumber, List<List<Job>> jobChains, List<Mashine> mashines)
        {
            for (var i = 0; i < _jobsSchedulingList[maxLedgeMashineNumber].Count; i++) // проходимо по всіх роботах в машині з мах виступом
            {
                for (var k = 0; k < _jobsSchedulingList.Count; k++) // проходимо по всіх роботах в пошуку обміну
                {
                    if (k != maxLedgeMashineNumber)
                    {
                        for (var l = 0; l < _jobsSchedulingList[k].Count; l++)
                        {
                            var delta_P = _jobsSchedulingList[maxLedgeMashineNumber][i].p - _jobsSchedulingList[k][l].p;  // дельта робіт

                            var maxLedgeCurrentMashine = _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C - delta_P * mashines[maxLedgeMashineNumber].k < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;

                            bool maxLedgeOtheMashine;

                            if (l == _jobsSchedulingList[k].Count - 1)
                            {
                                maxLedgeOtheMashine = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + delta_P * mashines[k].k < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;
                            }
                            else
                            {
                                var tmp_delta = delta_P * mashines[k].k - (_jobsSchedulingList[k][l + 1].C - _jobsSchedulingList[k][l + 1].p * mashines[k].k - _jobsSchedulingList[k][l].C);
                                if (tmp_delta < 0)
                                {
                                    tmp_delta = 0;
                                }
                                maxLedgeOtheMashine = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + (tmp_delta) < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;
                            }

                            Job carrentJob = new Job();
                            Job previousJob = new Job();
                            Job nextJob = new Job();
                            string[] jobNumber;

                            int canSwap = 0;
                            if (maxLedgeCurrentMashine && maxLedgeOtheMashine)
                            {
                                var tmp_jobChains = new List<List<Job>>();
                                var tmp_jobsSchedulingList = new List<List<Job>>();

                                for (int x = 0; x < jobChains.Count; x++)
                                {
                                    tmp_jobChains.Insert(x, new List<Job>());

                                    foreach (var job in jobChains[x])
                                    {
                                        tmp_jobChains[x].Add((Job)job.Clone());
                                    }
                                }

                                for (int x = 0; x < _jobsSchedulingList.Count; x++)
                                {
                                    tmp_jobsSchedulingList.Insert(x, new List<Job>());

                                    foreach (var job in _jobsSchedulingList[x])
                                    {
                                        jobNumber = job.Number.Split(',');

                                        tmp_jobsSchedulingList[x].Add(tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 1]);
                                    }
                                }

                                // ------  tmp SwapJobs ------

                                var job1 = tmp_jobsSchedulingList[maxLedgeMashineNumber][i];
                                var job2 = tmp_jobsSchedulingList[k][l];
                                var job2tmp = (Job)job2.Clone();

                                job2.C = job1.C - delta_P * mashines[maxLedgeMashineNumber].k;
                                job1.C = job2tmp.C + delta_P * mashines[k].k;

                                tmp_jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);
                                tmp_jobsSchedulingList[maxLedgeMashineNumber].Insert(i, job2);

                                tmp_jobsSchedulingList[k].RemoveAt(l);
                                tmp_jobsSchedulingList[k].Insert(l, job1);

                                for (int a = i + 1; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                {
                                    tmp_jobsSchedulingList[maxLedgeMashineNumber][a].C -= delta_P * mashines[maxLedgeMashineNumber].k;
                                }

                                for (int a = l + 1; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                {
                                    var oldStart = tmp_jobsSchedulingList[k][a].C - tmp_jobsSchedulingList[k][a].p * mashines[k].k;

                                    var tmp_C = tmp_jobsSchedulingList[k][a - 1].C - oldStart;
                                    if (tmp_C < 0)
                                    {
                                        tmp_C = 0;
                                    }

                                    tmp_jobsSchedulingList[k][a].C += tmp_C;
                                }

                                /// --- check  ---

                                for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                {
                                    carrentJob = tmp_jobsSchedulingList[maxLedgeMashineNumber][a]; // для тої що тепер з мах машини
                                    jobNumber = carrentJob.Number.Split(',');

                                    if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                    {
                                        previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                        if (previousJob.C > carrentJob.C - carrentJob.p * mashines[maxLedgeMashineNumber].k)
                                        {
                                            canSwap++;
                                        }
                                    }
                                    if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                    {
                                        nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                        for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                        {
                                            if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                            {
                                                if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                {
                                                    canSwap++;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (canSwap == 0)
                                {
                                    for (int a = l; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                    {
                                        carrentJob = tmp_jobsSchedulingList[k][a]; // для тої що тепер з іншої машини
                                        jobNumber = carrentJob.Number.Split(',');

                                        if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                        {
                                            previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                            if (previousJob.C > carrentJob.C - carrentJob.p * mashines[k].k)
                                            {
                                                canSwap++;
                                            }
                                        }
                                        if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                        {
                                            nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                            for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                            {
                                                if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                {
                                                    if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                    {
                                                        canSwap++;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (canSwap == 0)
                                {
                                    // ------   SwapJobs ------

                                    job1 = _jobsSchedulingList[maxLedgeMashineNumber][i];
                                    job2 = _jobsSchedulingList[k][l];
                                    job2tmp = (Job)job2.Clone();

                                    job2.C = job1.C - delta_P * mashines[maxLedgeMashineNumber].k;
                                    job1.C = job2tmp.C + delta_P * mashines[k].k;

                                    _jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);
                                    _jobsSchedulingList[maxLedgeMashineNumber].Insert(i, job2);

                                    _jobsSchedulingList[k].RemoveAt(l);
                                    _jobsSchedulingList[k].Insert(l, job1);

                                    for (int a = i + 1; a < _jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                    {
                                        _jobsSchedulingList[maxLedgeMashineNumber][a].C -= delta_P * mashines[maxLedgeMashineNumber].k;
                                    }

                                    for (int a = l + 1; a < _jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                    {
                                        var oldStart = _jobsSchedulingList[k][a].C - _jobsSchedulingList[k][a].p * mashines[k].k;

                                        var tmp_C = _jobsSchedulingList[k][a - 1].C - oldStart;
                                        if (tmp_C < 0)
                                        {
                                            tmp_C = 0;
                                        }

                                        _jobsSchedulingList[k][a].C += tmp_C;
                                    }

                                    mashines[maxLedgeMashineNumber].T = _jobsSchedulingList[maxLedgeMashineNumber].Count == 0 ? 0 : (decimal)(_jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C);
                                    mashines[k].T = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C == 0 ? 0 : (decimal)(_jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C);

                                    return true;
                                }
                            }

                            if (_jobsSchedulingList[k].Count == 1)
                            {
                                var reserv1 = _jobsSchedulingList[k][l].C - _jobsSchedulingList[k][l].p * mashines[k].k;
                                int reservStart1 = 0;

                                for (int r = reservStart1; r <= reserv1; r++)
                                {
                                    var tmp_delta3 = _jobsSchedulingList[maxLedgeMashineNumber][i].p * mashines[k].k - (reserv1 - r);
                                    if (tmp_delta3 < 0)
                                    {
                                        tmp_delta3 = 0;
                                    }
                                    var maxLedgeOtheMashineWithCurrentFirst = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + (tmp_delta3) < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;

                                    if (maxLedgeOtheMashineWithCurrentFirst)
                                    {
                                        canSwap = 0;

                                        var tmp_jobChains = new List<List<Job>>();
                                        var tmp_jobsSchedulingList = new List<List<Job>>();

                                        for (int x = 0; x < jobChains.Count; x++)
                                        {
                                            tmp_jobChains.Insert(x, new List<Job>());

                                            foreach (var job in jobChains[x])
                                            {
                                                tmp_jobChains[x].Add((Job)job.Clone());
                                            }
                                        }

                                        for (int x = 0; x < _jobsSchedulingList.Count; x++)
                                        {
                                            tmp_jobsSchedulingList.Insert(x, new List<Job>());

                                            foreach (var job in _jobsSchedulingList[x])
                                            {
                                                jobNumber = job.Number.Split(',');

                                                tmp_jobsSchedulingList[x].Add(tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 1]);
                                            }
                                        }

                                        // ------  tmp SwapJobs ------

                                        var job1 = tmp_jobsSchedulingList[maxLedgeMashineNumber][i];

                                        job1.C = r + job1.p * mashines[k].k;

                                        tmp_jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);

                                        tmp_jobsSchedulingList[k].Insert(0, job1);

                                        for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                        {
                                            tmp_jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                        }

                                        for (int a = 1; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                        {
                                            var oldStart = tmp_jobsSchedulingList[k][a].C - tmp_jobsSchedulingList[k][a].p * mashines[k].k;

                                            var tmp_C = tmp_jobsSchedulingList[k][a - 1].C - oldStart;
                                            if (tmp_C < 0)
                                            {
                                                tmp_C = 0;
                                            }

                                            tmp_jobsSchedulingList[k][a].C += tmp_C;
                                        }

                                        /// --- check  ---

                                        for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                        {
                                            carrentJob = tmp_jobsSchedulingList[maxLedgeMashineNumber][a]; // для тої що тепер з мах машини
                                            jobNumber = carrentJob.Number.Split(',');

                                            if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                            {
                                                previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                                if (previousJob.C > carrentJob.C - carrentJob.p * mashines[maxLedgeMashineNumber].k)
                                                {
                                                    canSwap++;
                                                }
                                            }
                                            if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                            {
                                                nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                                for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                                {
                                                    if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                    {
                                                        if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                        {
                                                            canSwap++;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (canSwap == 0)
                                        {
                                            for (int a = 0; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                            {
                                                carrentJob = tmp_jobsSchedulingList[k][a]; // для тої що тепер з іншої машини
                                                jobNumber = carrentJob.Number.Split(',');

                                                if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                                {
                                                    previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                                    if (previousJob.C > carrentJob.C - carrentJob.p * mashines[k].k)
                                                    {
                                                        canSwap++;
                                                    }
                                                }
                                                if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                                {
                                                    nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                                    for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                                    {
                                                        if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                        {
                                                            if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                            {
                                                                canSwap++;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (canSwap == 0)
                                        {
                                            // ------   SwapJobs ------

                                            job1 = _jobsSchedulingList[maxLedgeMashineNumber][i];

                                            job1.C = r + job1.p * mashines[k].k;

                                            _jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);

                                            _jobsSchedulingList[k].Insert(0, job1);

                                            for (int a = i; a < _jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                            {
                                                _jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                            }

                                            for (int a = 1; a < _jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                            {
                                                var oldStart = _jobsSchedulingList[k][a].C - _jobsSchedulingList[k][a].p * mashines[k].k;

                                                var tmp_C = _jobsSchedulingList[k][a - 1].C - oldStart;
                                                if (tmp_C < 0)
                                                {
                                                    tmp_C = 0;
                                                }

                                                _jobsSchedulingList[k][a].C += tmp_C;
                                            }

                                            mashines[maxLedgeMashineNumber].T = _jobsSchedulingList[maxLedgeMashineNumber].Count == 0 ? 0 : (decimal)(_jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C);
                                            mashines[k].T = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C == 0 ? 0 : (decimal)(_jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C);

                                            return true;
                                        }
                                    }
                                }

                            }

                            bool maxLedgeOtheMashineWithCurrent;

                            if (l == _jobsSchedulingList[k].Count - 1)
                            {
                                maxLedgeOtheMashineWithCurrent = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + _jobsSchedulingList[maxLedgeMashineNumber][i].p * mashines[k].k < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;
                            }
                            else
                            {
                                var tmp_delta2 = _jobsSchedulingList[maxLedgeMashineNumber][i].p * mashines[k].k - (_jobsSchedulingList[k][l + 1].C - _jobsSchedulingList[k][l + 1].p * mashines[k].k - _jobsSchedulingList[k][l].C);
                                if (tmp_delta2 < 0)
                                {
                                    tmp_delta2 = 0;
                                }
                                maxLedgeOtheMashineWithCurrent = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + (tmp_delta2) < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;
                            }

                            if (maxLedgeOtheMashineWithCurrent)
                            {
                                canSwap = 0;

                                var tmp_jobChains = new List<List<Job>>();
                                var tmp_jobsSchedulingList = new List<List<Job>>();

                                for (int x = 0; x < jobChains.Count; x++)
                                {
                                    tmp_jobChains.Insert(x, new List<Job>());

                                    foreach (var job in jobChains[x])
                                    {
                                        tmp_jobChains[x].Add((Job)job.Clone());
                                    }
                                }

                                for (int x = 0; x < _jobsSchedulingList.Count; x++)
                                {
                                    tmp_jobsSchedulingList.Insert(x, new List<Job>());

                                    foreach (var job in _jobsSchedulingList[x])
                                    {
                                        jobNumber = job.Number.Split(',');

                                        tmp_jobsSchedulingList[x].Add(tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 1]);
                                    }
                                }

                                // ------  tmp SwapJobs ------

                                var job1 = tmp_jobsSchedulingList[maxLedgeMashineNumber][i];

                                job1.C = tmp_jobsSchedulingList[k][l].C + job1.p * mashines[k].k;

                                tmp_jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);

                                tmp_jobsSchedulingList[k].Insert(l + 1, job1);

                                for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                {
                                    tmp_jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                }

                                for (int a = l + 2; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                {
                                    var oldStart = tmp_jobsSchedulingList[k][a].C - tmp_jobsSchedulingList[k][a].p * mashines[k].k;

                                    var tmp_C = tmp_jobsSchedulingList[k][a - 1].C - oldStart;
                                    if (tmp_C < 0)
                                    {
                                        tmp_C = 0;
                                    }

                                    tmp_jobsSchedulingList[k][a].C += tmp_C;
                                }

                                /// --- check  ---

                                for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                {
                                    carrentJob = tmp_jobsSchedulingList[maxLedgeMashineNumber][a]; // для тої що тепер з мах машини
                                    jobNumber = carrentJob.Number.Split(',');

                                    if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                    {
                                        previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                        if (previousJob.C > carrentJob.C - carrentJob.p * mashines[maxLedgeMashineNumber].k)
                                        {
                                            canSwap++;
                                        }
                                    }
                                    if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                    {
                                        nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                        for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                        {
                                            if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                            {
                                                if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                {
                                                    canSwap++;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (canSwap == 0)
                                {
                                    for (int a = l; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                    {
                                        carrentJob = tmp_jobsSchedulingList[k][a]; // для тої що тепер з іншої машини
                                        jobNumber = carrentJob.Number.Split(',');

                                        if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                        {
                                            previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                            if (previousJob.C > carrentJob.C - carrentJob.p * mashines[k].k)
                                            {
                                                canSwap++;
                                            }
                                        }
                                        if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                        {
                                            nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                            for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                            {
                                                if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                {
                                                    if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                    {
                                                        canSwap++;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (canSwap == 0)
                                {
                                    // ------   SwapJobs ------

                                    job1 = _jobsSchedulingList[maxLedgeMashineNumber][i];

                                    job1.C = _jobsSchedulingList[k][l].C + job1.p * mashines[k].k;

                                    _jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);

                                    _jobsSchedulingList[k].Insert(l + 1, job1);

                                    for (int a = i; a < _jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                    {
                                        _jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                    }

                                    for (int a = l + 2; a < _jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                    {
                                        var oldStart = _jobsSchedulingList[k][a].C - _jobsSchedulingList[k][a].p * mashines[k].k;

                                        var tmp_C = _jobsSchedulingList[k][a - 1].C - oldStart;
                                        if (tmp_C < 0)
                                        {
                                            tmp_C = 0;
                                        }

                                        _jobsSchedulingList[k][a].C += tmp_C;
                                    }

                                    mashines[maxLedgeMashineNumber].T = _jobsSchedulingList[maxLedgeMashineNumber].Count == 0 ? 0 : (decimal)(_jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C);
                                    mashines[k].T = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C == 0 ? 0 : (decimal)(_jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C);

                                    return true;
                                }
                            }

                            decimal? reserv = 0;
                            int reservStart = 0;
                            if (l == _jobsSchedulingList[k].Count - 1)
                            {
                                reserv = _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C - _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C;
                                reservStart = Convert.ToInt32(_jobsSchedulingList[k][l].C) + 1;
                            }
                            else
                            {
                                reserv = _jobsSchedulingList[k][l + 1].C - _jobsSchedulingList[k][l + 1].p * mashines[k].k - _jobsSchedulingList[k][l].C;
                                reservStart = Convert.ToInt32(_jobsSchedulingList[k][l].C) + 1;
                            }

                            for (int r = 0; r < reserv; r++)
                            {
                                if (l == _jobsSchedulingList[k].Count - 1)
                                {
                                    maxLedgeOtheMashineWithCurrent = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + _jobsSchedulingList[maxLedgeMashineNumber][i].p * mashines[k].k + r < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;
                                }
                                else
                                {
                                    var tmp_delta2 = _jobsSchedulingList[maxLedgeMashineNumber][i].p * mashines[k].k - (reserv - r);
                                    if (tmp_delta2 < 0)
                                    {
                                        tmp_delta2 = 0;
                                    }
                                    maxLedgeOtheMashineWithCurrent = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C + (tmp_delta2) < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;
                                }

                                if (maxLedgeOtheMashineWithCurrent)
                                {
                                    canSwap = 0;

                                    var tmp_jobChains = new List<List<Job>>();
                                    var tmp_jobsSchedulingList = new List<List<Job>>();

                                    for (int x = 0; x < jobChains.Count; x++)
                                    {
                                        tmp_jobChains.Insert(x, new List<Job>());

                                        foreach (var job in jobChains[x])
                                        {
                                            tmp_jobChains[x].Add((Job)job.Clone());
                                        }
                                    }

                                    for (int x = 0; x < _jobsSchedulingList.Count; x++)
                                    {
                                        tmp_jobsSchedulingList.Insert(x, new List<Job>());

                                        foreach (var job in _jobsSchedulingList[x])
                                        {
                                            jobNumber = job.Number.Split(',');

                                            tmp_jobsSchedulingList[x].Add(tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 1]);
                                        }
                                    }

                                    // ------  tmp SwapJobs ------

                                    var job1 = tmp_jobsSchedulingList[maxLedgeMashineNumber][i];

                                    job1.C = tmp_jobsSchedulingList[k][l].C + r + job1.p * mashines[k].k;

                                    tmp_jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);

                                    tmp_jobsSchedulingList[k].Insert(l + 1, job1);

                                    for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                    {
                                        tmp_jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                    }

                                    for (int a = l + 2; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                    {
                                        var oldStart = tmp_jobsSchedulingList[k][a].C - tmp_jobsSchedulingList[k][a].p * mashines[k].k;

                                        var tmp_C = tmp_jobsSchedulingList[k][a - 1].C - oldStart;
                                        if (tmp_C < 0)
                                        {
                                            tmp_C = 0;
                                        }

                                        tmp_jobsSchedulingList[k][a].C += tmp_C;
                                    }

                                    /// --- check  ---

                                    for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                    {
                                        carrentJob = tmp_jobsSchedulingList[maxLedgeMashineNumber][a]; // для тої що тепер з мах машини
                                        jobNumber = carrentJob.Number.Split(',');

                                        if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                        {
                                            previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                            if (previousJob.C > carrentJob.C - carrentJob.p * mashines[maxLedgeMashineNumber].k)
                                            {
                                                canSwap++;
                                            }
                                        }
                                        if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                        {
                                            nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                            for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                            {
                                                if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                {
                                                    if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                    {
                                                        canSwap++;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (canSwap == 0)
                                    {
                                        for (int a = l; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                        {
                                            carrentJob = tmp_jobsSchedulingList[k][a]; // для тої що тепер з іншої машини
                                            jobNumber = carrentJob.Number.Split(',');

                                            if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                            {
                                                previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                                if (previousJob.C > carrentJob.C - carrentJob.p * mashines[k].k)
                                                {
                                                    canSwap++;
                                                }
                                            }
                                            if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                            {
                                                nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                                for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                                {
                                                    if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                    {
                                                        if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                        {
                                                            canSwap++;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (canSwap == 0)
                                    {
                                        // ------   SwapJobs ------

                                        job1 = _jobsSchedulingList[maxLedgeMashineNumber][i];

                                        job1.C = _jobsSchedulingList[k][l].C + r + job1.p * mashines[k].k;

                                        _jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);

                                        _jobsSchedulingList[k].Insert(l + 1, job1);

                                        for (int a = i; a < _jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                        {
                                            _jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                        }

                                        for (int a = l + 2; a < _jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                        {
                                            var oldStart = _jobsSchedulingList[k][a].C - _jobsSchedulingList[k][a].p * mashines[k].k;

                                            var tmp_C = _jobsSchedulingList[k][a - 1].C - oldStart;
                                            if (tmp_C < 0)
                                            {
                                                tmp_C = 0;
                                            }

                                            _jobsSchedulingList[k][a].C += tmp_C;
                                        }

                                        mashines[maxLedgeMashineNumber].T = _jobsSchedulingList[maxLedgeMashineNumber].Count == 0 ? 0 : (decimal)(_jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C);
                                        mashines[k].T = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C == 0 ? 0 : (decimal)(_jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C);

                                        return true;
                                    }
                                }
                            }
                        }

                        if (_jobsSchedulingList[k].Count == 0)
                        {
                            Job carrentJob = new Job();
                            Job previousJob = new Job();
                            Job nextJob = new Job();
                            string[] jobNumber;

                            var maxLedgeOtheMashineWithCurrent = _jobsSchedulingList[maxLedgeMashineNumber][i].p * mashines[k].k < _jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C;

                            int canSwap = 0;
                            if (maxLedgeOtheMashineWithCurrent)
                            {
                                var tmp_jobChains = new List<List<Job>>();
                                var tmp_jobsSchedulingList = new List<List<Job>>();

                                for (int x = 0; x < jobChains.Count; x++)
                                {
                                    tmp_jobChains.Insert(x, new List<Job>());

                                    foreach (var job in jobChains[x])
                                    {
                                        tmp_jobChains[x].Add((Job)job.Clone());
                                    }
                                }

                                for (int x = 0; x < _jobsSchedulingList.Count; x++)
                                {
                                    tmp_jobsSchedulingList.Insert(x, new List<Job>());

                                    foreach (var job in _jobsSchedulingList[x])
                                    {
                                        jobNumber = job.Number.Split(',');

                                        tmp_jobsSchedulingList[x].Add(tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 1]);
                                    }
                                }

                                // ------  tmp SwapJobs ------

                                var job1 = tmp_jobsSchedulingList[maxLedgeMashineNumber][i];

                                tmp_jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);
                                tmp_jobsSchedulingList[k].Add(job1);
                                tmp_jobsSchedulingList[k][0].C = job1.p * mashines[k].k;

                                for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                {
                                    tmp_jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                }

                                /// --- check  ---

                                for (int a = i; a < tmp_jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                {
                                    carrentJob = tmp_jobsSchedulingList[maxLedgeMashineNumber][a]; // для тої що тепер з мах машини
                                    jobNumber = carrentJob.Number.Split(',');

                                    if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                    {
                                        previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                        if (previousJob.C > carrentJob.C - carrentJob.p * mashines[maxLedgeMashineNumber].k)
                                        {
                                            canSwap++;
                                        }
                                    }
                                    if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                    {
                                        nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                        for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                        {
                                            if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                            {
                                                if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                {
                                                    canSwap++;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (canSwap == 0)
                                {
                                    for (int a = 0; a < tmp_jobsSchedulingList[k].Count; a++)   // всі роботи з машини іншої
                                    {
                                        carrentJob = tmp_jobsSchedulingList[k][a]; // для тої що тепер з іншої машини
                                        jobNumber = carrentJob.Number.Split(',');

                                        if (Convert.ToInt32(jobNumber[1]) != 1) // якщо не перша
                                        {
                                            previousJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1]) - 2];

                                            if (previousJob.C > carrentJob.C - carrentJob.p * mashines[k].k)
                                            {
                                                canSwap++;
                                            }
                                        }
                                        if (tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1].Count - 1 != Convert.ToInt32(jobNumber[1]) - 1) // якщо не остання
                                        {
                                            nextJob = tmp_jobChains[Convert.ToInt32(jobNumber[0]) - 1][Convert.ToInt32(jobNumber[1])];

                                            for (int m = 0; m < tmp_jobsSchedulingList.Count; m++)
                                            {
                                                if (tmp_jobsSchedulingList[m].Contains(nextJob))
                                                {
                                                    if (nextJob.C - nextJob.p * mashines[m].k < carrentJob.C)
                                                    {
                                                        canSwap++;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (canSwap == 0)
                                {
                                    // ------   SwapJobs ------

                                    job1 = _jobsSchedulingList[maxLedgeMashineNumber][i];

                                    _jobsSchedulingList[maxLedgeMashineNumber].RemoveAt(i);
                                    _jobsSchedulingList[k].Add(job1);
                                    _jobsSchedulingList[k][0].C = job1.p * mashines[k].k;

                                    for (int a = i; a < _jobsSchedulingList[maxLedgeMashineNumber].Count; a++) // всі роботи з машини з найб виступом
                                    {
                                        _jobsSchedulingList[maxLedgeMashineNumber][a].C -= job1.p * mashines[maxLedgeMashineNumber].k;
                                    }

                                    mashines[maxLedgeMashineNumber].T = _jobsSchedulingList[maxLedgeMashineNumber].Count == 0 ? 0 : (decimal)(_jobsSchedulingList[maxLedgeMashineNumber][_jobsSchedulingList[maxLedgeMashineNumber].Count - 1].C);
                                    mashines[k].T = _jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C == 0 ? 0 : (decimal)(_jobsSchedulingList[k][_jobsSchedulingList[k].Count - 1].C);

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
