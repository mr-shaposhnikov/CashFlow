using CashFlow.Core.Models;
using CashFlow.Core.Repositories;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace CashFlow
{
    public class Startup
    {
        // TODO: избавиться от ссылок на IoC по проекту
        public static WindsorContainer IoContainer { get; } = BootstrapIoContainer();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            // TODO: Json ошибку в ответ возвращать

            app.UseMvc();
        }

        private static WindsorContainer BootstrapIoContainer()
        {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + System.AppDomain.CurrentDomain.BaseDirectory + "Data\\cashflow.mdf" + ";Integrated Security=True;";

            var container = new WindsorContainer();

            container.Register(Component
                .For<IConnectionString>()
                .UsingFactoryMethod(kernel => new ConnectionString(connectionString))
                .LifeStyle.Singleton);

            container.Register(Component
                .For<ISessionFactory>()
                .UsingFactoryMethod(kernel =>
                {
                    // For: FluentNHibernate
                    var connstr = kernel.Resolve<IConnectionString>().Value;
                    return Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connstr).ShowSql())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Cost>())
                        .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(useStdOut: true, execute: true, justDrop: false))
                        .BuildSessionFactory();

                    //// From: https://habrahabr.ru/post/265371/
                    //var cfg = new Configuration()
                    //    .DataBaseIntegration(d =>
                    //    {
                    //        d.ConnectionString = kernel.Resolve<IConnectionString>().Value;
                    //        d.Dialect<MsSql2012Dialect>();
                    //    });
                    //var mapper = new ModelMapper();
                    //mapper.AddMappings(Assembly.GetAssembly(typeof(Cost)).GetExportedTypes());
                    //var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                    //cfg.AddMapping(mapping);
                    //new SchemaUpdate(cfg).Execute(true, true);
                    //return cfg.BuildSessionFactory();
                })
                .LifeStyle.Singleton);

            // TODO: использовать LifeStyle.PerWebRequest вместо LifeStyle.Singleton для репозиториев и сессии

            container.Register(Component
                .For<NHibernate.ISession>()
                .UsingFactoryMethod(kernel => kernel.Resolve<ISessionFactory>().OpenSession())
                .LifeStyle.Transient);

            container.Register(Component
                .For<ICategoryRepository>()
                .ImplementedBy<CategoryRepository>()
                .LifeStyle.Transient);

            container.Register(Component
                .For<ICashRepository>()
                .ImplementedBy<CashRepository>()
                .LifeStyle.Transient);

            container.Register(Component
                .For<ICostRepository>()
                .ImplementedBy<CostRepository>()
                .LifeStyle.Transient);

            return container;
        }
    }
}
