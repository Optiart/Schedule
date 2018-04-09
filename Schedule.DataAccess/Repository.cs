using System;
using System.Linq;

namespace Schedule.DataAccess
{
    public class Repository : IRepository
    {
        public Tabs[] GetAll()
        {
            using (var context = new ScheduleDbContext())
            {
                return context.Tabs.ToArray();
            }
        }

        public void Save(params Tabs[] tabs)
        {
            using (var context = new ScheduleDbContext())
            {
                context.Tabs.AddRange(tabs);
                context.SaveChanges();
            }
        }

        public void Delete(params Tabs[] tabs)
        {
            using (var context = new ScheduleDbContext())
            {
                context.Tabs.RemoveRange(tabs);
                context.SaveChanges();
            }
        }

        public void Update(Tabs tab)
        {
            using (var context = new ScheduleDbContext())
            {
                Tabs targetTab = context.Tabs.Find(tab.id);
                if (targetTab == null)
                {
                    throw new InvalidOperationException($"Tab {tab.id} does not exist");
                }

                context.Entry(targetTab).CurrentValues.SetValues(tab);
                context.SaveChanges();
            }
        }
    }
}
