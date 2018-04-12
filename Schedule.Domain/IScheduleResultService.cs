using Schedule.Domain.Models;
using Schedule.Models;

namespace Schedule.Domain
{
    public interface IScheduleResultService
    {
        Result Calculate(Tab tab);
    }
}