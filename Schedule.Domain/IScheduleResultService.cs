using Schedule.Domain.Models;
using Schedule.Models;

namespace Schedule.Domain
{
    public interface IScheduleResultService
    {
        void Process(Tab tab);

        Result GetResult(int tabId);
    }
}