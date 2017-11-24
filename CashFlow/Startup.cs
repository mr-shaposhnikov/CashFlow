using CashFlow.Core.Models;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public static readonly WindsorContainer IoContainer = BootstrapIoContainer();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var s = IoContainer.Resolve<NHibernate.ISession>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
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

            container.Register(Component
                .For<NHibernate.ISession>()
                .UsingFactoryMethod(kernel => kernel.Resolve<ISessionFactory>().OpenSession())
                );
                // TODO: .LifeStyle.PerWebRequest);

            return container;
        }

        //private static NHibernate.Cfg.Configuration ConfigureNHibernate()
        //{
        //    var cfg = new NHibernate.Cfg.Configuration();
        //    cfg.SessionFactoryName("BuildIt");
        //    cfg.DataBaseIntegration(db =>
        //    {
        //        db.Dialect();
        //        db.Driver();
        //        db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
        //        db.IsolationLevel = IsolationLevel.ReadCommitted;

        //        db.ConnectionStringName = "NH3";
        //        db.Timeout = 10;

        //        // enabled for testing
        //        db.LogFormattedSql = true;
        //        db.LogSqlInConsole = true;
        //        db.AutoCommentSql = true;
        //    });

        //    var mapping = GetMappings();
        //    cfg.AddDeserializedMapping(mapping, "NHSchemaCashFlow");
        //    SchemaMetadataUpdater.QuoteTableAndColumns(cfg);

        //    return cfg;
        //}
    }
}
