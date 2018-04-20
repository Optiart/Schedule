using System;
using System.Linq;

namespace Schedule.DataAccess
{
    internal class ResultRepository : IResultRepository
    {
        public (ResultDto Result, AlgorithmSummaryDto[] Summaries) GetByTab(int tabId)
        {
            using (var context = new ScheduleDbContext())
            {
                ResultDto result = context.Results
                    .Where(r => r.TabId == tabId)
                    .FirstOrDefault();

                if (result == null)
                {
                    throw new InvalidOperationException("No result for tab available");
                }

                AlgorithmSummaryDto[] summaries = context.AlgorithmSummaries
                    .Where(a => a.ResultId == result.Id)
                    .ToArray();

                return (Result: result, Summaries: summaries);
            }
        }

        public void Save(ResultDto result, AlgorithmSummaryDto[] summaries)
        {
            using (var context = new ScheduleDbContext())
            {
                context.Results.Add(result);
                context.SaveChanges();

                foreach (var summary in summaries)
                {
                    summary.ResultId = result.Id;
                    context.AlgorithmSummaries.Add(summary);
                }

                context.SaveChanges();
            }
        }
    }
}
