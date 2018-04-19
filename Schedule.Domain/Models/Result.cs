using System.Collections.Generic;

namespace Schedule.Domain.Models
{
    public class Result
    {
        public int TabId { get; set; }

        public Dictionary<int, PlotRowPerDevice[]> GraphData { get; set; }
    }
}