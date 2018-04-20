namespace Schedule.Domain.Models
{
    public class AlgorithSummary
    {
        public AlgorithmType Type { get; set; }

        public decimal Cstar { get; set; }

        public decimal Cmax { get; set; }
    }
}