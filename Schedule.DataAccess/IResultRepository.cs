namespace Schedule.DataAccess
{
    public interface IResultRepository
    {
        (ResultDto Result, AlgorithmSummaryDto[] Summaries) GetByTab(int tabId);

        void Save(ResultDto Result, AlgorithmSummaryDto[] Summaries);
    }
}