using System;
using System.Linq;

namespace Schedule.DataAccess
{
    internal class TabRepository : ITabRepository
    {
        public int[] GetAllTabIds()
        {
            using (var context = new ScheduleDbContext())
            {
                return context.Tabs.Select(t => t.Id).ToArray();
            }
        }

        public TabDto[] GetAll()
        {
            using (var context = new ScheduleDbContext())
            {
                return context.Tabs.ToArray();
            }
        }

        public void Save(TabDto tab)
        {
            using (var context = new ScheduleDbContext())
            {
                context.Tabs.Add(tab);
                context.SaveChanges();
            }
        }

        public void Delete(int tabId)
        {
            using (var context = new ScheduleDbContext())
            {
                TabDto tab = context.Tabs.Find(tabId);
                context.Tabs.Remove(tab);
                context.SaveChanges();
            }
        }

        public void Update(TabDto tab)
        {
            using (var context = new ScheduleDbContext())
            {
                TabDto targetTab = context.Tabs.Find(tab.Id);
                if (targetTab == null)
                {
                    throw new InvalidOperationException($"Tab {tab.Id} does not exist");
                }

                context.Entry(targetTab).CurrentValues.SetValues(tab);
                context.SaveChanges();
            }
        }
    }
}
