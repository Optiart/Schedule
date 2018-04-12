using System;
using System.Linq;

namespace Schedule.DataAccess
{
    internal class Repository : IRepository
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

        public void Delete(int tabId)
        {
            using (var context = new ScheduleDbContext())
            {
                Tabs tab = context.Tabs.Find(tabId);
                context.Tabs.Remove(tab);
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
