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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            // запускаем миграции
            migrationRunner.MigrateUp();
        }
    }
}