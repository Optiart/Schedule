using Schedule.Models;

namespace Schedule.Domain
{
    public interface ITabService
    {
        int[] GetAllTabIds();

        Tab[] GetAll();

        int Save(Tab tabModel);

        void Delete(int id);
    }
}