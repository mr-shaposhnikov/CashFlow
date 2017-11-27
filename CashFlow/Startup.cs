using System;
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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace CashFlow
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");
            AppConfiguration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // WARNING: Исправляем ошибку, из-за которой вместо массива трат CostController.Get() возвращал только первую трату
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            
            IoContainerConfigure(services);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            // TODO: Json ошибку в ответ возвращать

            app.UseMvc();
        }

        private void IoContainerConfigure(IServiceCollection services)
        {
            services.AddSingleton<IConnectionString, ConnectionString>(srvProvider =>
            {
                var connectionStringSection = AppConfiguration.GetSection("ConnectionStrings");
                var connectionString = connectionStringSection.GetSection("DefaultConnection").Value ?? throw new ArgumentNullException("Connection string not found!");
                return new ConnectionString(connectionString);
            });

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
        }
    }
}
