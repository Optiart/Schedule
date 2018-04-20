using Schedule.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.Models
{
    public class ResultViewModel
    {
        public int Id { get; set; }

        public int CurrentTab { get; set; }

        public int[] TabIds { get; set; }

        public decimal[,] Chain { get; set; }

        public AlgorithSummary[] AlgorithSummaries { get; set; }

        public Dictionary<AlgorithmType, Plot> PlotData { get; set; }

        public Dictionary<int, string> ColorByPalleteWork { get; private set; }

        public Dictionary<AlgorithmType, decimal> MaxDurationByAlgorithmType =>
            PlotData.ToDictionary(
                data => data.Key,
                data => data.Value.Max(kvp => kvp.Value.Max(g => g.End)));

        public ResultViewModel()
        {
            DefineColors();
        }

        private void DefineColors()
        {
            ColorByPalleteWork = new Dictionary<int, string>
                    {
                        { 1, "#FF6A60" },
                        { 2, "#FB9056" },
                        { 3, "#FFB543" },
                        { 4, "#FFEF52" },
                        { 5, "#C0FB38" },

                        { 6, "#81E950" },
                        { 7, "#2DB429" },
                        { 8, "#65D183" },
                        { 9, "#2ACE9A" },
                        { 10, "#78B1BF" },

                        { 11, "#7C8CA2" },
                        { 12, "#8067E7" },
                        { 13, "#A652FF" },
                        { 14, "#FF80FD" },
                        { 15, "#AE788A" },

                        { 16, "#C89B9B" },
                        { 17, "#9A9A9A" },
                        { 18, "#9E9E40" },
                        { 19, "#3E8584" },
                        { 20, "#438441" },
                    };
        }
    }
}