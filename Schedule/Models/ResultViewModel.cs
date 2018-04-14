using Schedule.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.Models
{
    public class ResultViewModel
    {
        public int Id { get; set; }

        public int TabId { get; set; }

        public decimal[,] Chain { get; set; }

        public Dictionary<int, DeviceGraphRow[]> GraphData { get; set; }

        public Dictionary<int, string> ColorByPalleteWork { get; }

        public int MaxDuration
        {
            get { return GraphData.Max(kvp => kvp.Value.Sum(v => v.Duration)); }
        }

        public ResultViewModel()
        {
            ColorByPalleteWork = new Dictionary<int, string>
                    {
                        { 1, "#478bf7" },
                        { 2, "#8c543a" },
                        { 3, "#094eba" },
                        { 4, "#6b83a8" },
                        { 5, "#f26321" }
                    };
        }
    }
}