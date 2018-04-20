namespace Schedule.DataAccess
{
    public interface ITabRepository
    {
        int[] GetAllTabIds();

        TabDto[] GetAll();

        void Save(TabDto tab);

        void Delete(int tab);

        void Update(TabDto tab);
    }
}