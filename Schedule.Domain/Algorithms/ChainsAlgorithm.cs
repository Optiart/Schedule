using System;
using System.Collections.Generic;

namespace Schedule.Domain.Algorithms
{
    static class ChainsAlgorithm
    {
        static int JobRow;
        static int JobColumn;

        public static List<List<int>> GetChains(int[,] jobPalette)
        {
            JobRow = jobPalette.GetLength(0);
            JobColumn = jobPalette.GetLength(1);
            
            //for (int i = 0; i < JobRow; i++)
            //{
            //    for (int j = 0; j < JobColumn; j++)
            //    {
            //        Console.Write(jobPalette[i, j] + "  ");                  
            //    }

            //    Console.WriteLine();
            //}

            var jobChains = SeparationPoint(jobPalette);

            //Console.WriteLine();
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine();

            //foreach (var chain in jobChains)
            //{
            //    foreach (var job in chain)
            //    {
            //        Console.Write(job + "  ");
            //    }
            //    Console.WriteLine();
            //}

            return jobChains;
        }

        static List<List<int>> SeparationPoint( int[,] jobPalette)
        {
            var jobChains = new List<List<int>>();

            var centralPoint = JobColumn / 2;

            int centralTimeLeftChain = 0;
            int centralTimeRightChain = 0;

            int leftTimeLeftChain = 0;
            int leftTimeRightChain = 0;

            int rightTimeLeftChain = 0;
            int rightTimeRightChain = 0;

            for (int i = 0; i < JobRow; i++)
            {              
                var currentPoint = centralPoint;

                GetTimeChain(jobPalette, i, currentPoint, ref centralTimeLeftChain, ref centralTimeRightChain);
                GetTimeChain(jobPalette, i, currentPoint - 1, ref leftTimeLeftChain, ref leftTimeRightChain);
                GetTimeChain(jobPalette, i, currentPoint + 1, ref rightTimeLeftChain, ref rightTimeRightChain);

                while (true)
                {
                    var centralResult = Math.Abs(centralTimeLeftChain - centralTimeRightChain);
                    var leftResult = Math.Abs(leftTimeLeftChain - leftTimeRightChain);
                    var rightResult = Math.Abs(rightTimeLeftChain - rightTimeRightChain);

                    if (centralResult <= leftResult && centralResult <= rightResult)
                    {
                        AddChain(jobChains, currentPoint, jobPalette, i);
                        break;
                    }

                    if (leftResult <= centralResult && leftResult <= rightResult)
                    {
                        currentPoint -= 1;

                        rightTimeLeftChain = centralTimeLeftChain;
                        rightTimeRightChain = centralTimeRightChain;

                        centralTimeLeftChain = leftTimeLeftChain;
                        centralTimeRightChain = leftTimeRightChain;                        

                        GetTimeChain(jobPalette, i, currentPoint - 1, ref leftTimeLeftChain, ref leftTimeRightChain);
                    }
                    else
                    {
                        currentPoint += 1;

                        leftTimeLeftChain = centralTimeLeftChain;
                        leftTimeRightChain = centralTimeRightChain;

                        centralTimeLeftChain = rightTimeLeftChain;
                        centralTimeRightChain = rightTimeRightChain;

                        GetTimeChain(jobPalette, i, currentPoint + 1, ref rightTimeLeftChain, ref rightTimeRightChain);
                    }                    
                }    
            }

            return jobChains;
        }

        private static void GetTimeChain(int[,] jobPalette, int jobPaletteRow, int currentPoint, ref int timeLeftChain, ref int timeRightChain)
        {
            timeLeftChain = 0;
            timeRightChain = 0;

            for (int q = 0; q < currentPoint; q++)
            {
                timeLeftChain += jobPalette[jobPaletteRow, q];
            }
            for (int q = currentPoint; q < JobColumn; q++)
            {
                timeRightChain += jobPalette[jobPaletteRow, q];
            }
        }

        private static void AddChain(List<List<int>> jobChain, int currentPoint, int[,] jobPalette, int jobPaletteRow)
        {
            var leftChain = new List<int>();
            var rightChain = new List<int>();

            for (int q = 0; q < currentPoint; q++)
            {
                leftChain.Add(jobPalette[jobPaletteRow, q]);
            }
            for (int q = currentPoint; q < JobColumn; q++)
            {
                rightChain.Add(jobPalette[jobPaletteRow, q]);
            }

            jobChain.Add(leftChain);
            jobChain.Add(rightChain);
        }

    }
}
