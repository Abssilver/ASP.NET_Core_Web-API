using System;
using AutoMapper;
using Dapper;
using FluentMigrator.Runner;
using MetricsManager.Client;
using MetricsManager.Client.Interfaces;
using MetricsManager.DataAccessLayer;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Repositories;
using MetricsManager.Jobs;
using MetricsManager.Jobs.JobFactory;
using MetricsManager.Jobs.Schedule;
using MetricsManager.MappingSettings;
using MetricsManager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsManager
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
            
            // добавляем наши задачи
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?"));
            
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

            services.AddSingleton<IAgentInfoRepository, AgentInfoRepository>();
            services.AddSingleton<ICpuMetricsManagerRepository,CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsManagerRepository,DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsManagerRepository,HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsManagerRepository,NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsManagerRepository,RamMetricsRepository>();
            services.AddSingleton<IDatabaseSettingsProvider, DatabaseSettingsProvider>();
            
            services.AddHttpClient<ICpuMetricsAgentClient, CpuMetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            
            services.AddHttpClient<IDotNetMetricsAgentClient, DotNetMetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            
            services.AddHttpClient<IHddMetricsAgentClient, HddMetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            
            services.AddHttpClient<INetworkMetricsAgentClient, NetworkMetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            
            services.AddHttpClient<IRamMetricsAgentClient, RamMetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            
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