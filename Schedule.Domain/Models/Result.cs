using System.Collections.Generic;

namespace Schedule.Domain.Models
{
    public class Result
    {
        public int Id { get; set; }

        public decimal[,] Chain { get; set; }

        public AlgorithSummary[] AlgorithSummaries { get; set; }

        public Dictionary<AlgorithmType, Plot> PlotData { get; set; }
    }
}