using CashFlow.Core.Models;
using CashFlow.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Newtonsoft.Json;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace CashFlow
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Исправляем ошибку, из-за которой вместо массива трат CostController.Get() возвращал только первую трату
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            #region IoC
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + System.AppDomain.CurrentDomain.BaseDirectory + "Data\\cashflow.mdf" + ";Integrated Security=True;";
            services.AddSingleton<IConnectionString, ConnectionString>(srvProvider => new ConnectionString(connectionString));

            services.AddSingleton(srvProvider =>
            {
                // For: FluentNHibernate
                var connstr = srvProvider.GetService<IConnectionString>().Value;
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
            });

            services.AddScoped(srvProvider => srvProvider.GetService<ISessionFactory>().OpenSession());

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICashRepository, CashRepository>();
            services.AddScoped<ICostRepository, CostRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            // TODO: Json ошибку в ответ возвращать

            app.UseMvc();
        }
    }
}
