namespace Schedule.DataAccess
{
    public interface IRepository
    {
        Tabs[] GetAll();

        void Save(params Tabs[] tabs);

        void Delete(int tab);

        void Update(Tabs tab);
    }
}