using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Schedule.DataAccess;
using Schedule.Domain;
using System.Web.Http;
using System.Web.Mvc;

namespace Schedule
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            builder.RegisterType<TabService>().As<ITabService>();
            builder.RegisterType<ScheduleResultService>().As<IScheduleResultService>();
            builder.RegisterType<TabRepository>().As<ITabRepository>();
            builder.RegisterType<ResultRepository>().As<IResultRepository>();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}