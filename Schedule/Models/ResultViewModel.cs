using Schedule.Domain.Models;
using System.Collections.Generic;

namespace Schedule.Models
{
    public class ResultViewModel
    {
        public int Id { get; set; }

        public int TabId { get; set; }

        public decimal[,] Chain { get; set; }

        public Dictionary<int, DeviceGraphRow[]> GraphData { get; set; }
    }
}