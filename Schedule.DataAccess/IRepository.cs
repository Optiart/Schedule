namespace Schedule.DataAccess
{
    public interface IRepository
    {
        Tabs[] GetAll();

        void Save(params Tabs[] tabs);

        void Delete(params Tabs[] tabs);

        void Update(Tabs tab);
    }
}