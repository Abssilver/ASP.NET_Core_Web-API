using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Dapper;
using FluentMigrator.Runner;
using MetricsAgent.DataAccessLayer;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Repositories;
using MetricsAgent.Jobs;
using MetricsAgent.Jobs.JobFactory;
using MetricsAgent.Jobs.Schedule;
using MetricsAgent.MappingSettings;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Добавляем сервисы
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            
            // добавляем нашу задачу
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд
            
            services.AddSingleton<DotNetMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: "0/5 * * * * ?"));
            
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: "0/5 * * * * ?"));
            
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: "0/5 * * * * ?"));
            
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();
            
            services.AddSingleton<ICpuMetricsRepository,CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository,DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository,HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository,NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository,RamMetricsRepository>();
            services.AddSingleton<IDatabaseSettingsProvider, DatabaseSettingsProvider>();
            
            ConfigureSwagger(services);
            
            ConfigureMapper(services);
            ConfigureMigration(services);
        }

        private void ConfigureMigration(IServiceCollection services)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(ConnectionString)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }
        
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API сервиса агента сбора метрик",
                    Description = "Тут можно поиграть с api нашего сервиса",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Joe The Cat",
                        Email = string.Empty,
                        Url = new Uri("https://gb.ru"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "можно указать под какой лицензией все опубликовано",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                // Указываем файл из которого брать комментарии для Swagger UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
        }

        private void ConfigureMapper(IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(
                mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса агента сбора метрик");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            // запускаем миграции
            migrationRunner.MigrateUp();
        }
    }
}