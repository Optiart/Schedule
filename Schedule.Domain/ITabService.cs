using Schedule.Models;

namespace Schedule.Domain
{
    public interface ITabService
    {
        Tab[] GetAll();

        void Save(Tab tabModel);

        void Delete(int id);
    }
}