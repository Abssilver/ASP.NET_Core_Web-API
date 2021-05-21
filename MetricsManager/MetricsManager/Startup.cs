using System;
using System.IO;
using System.Reflection;
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
using Microsoft.OpenApi.Models;
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

            ConfigureClient(services);
            ConfigureMapper(services);
            ConfigureMigration(services);
            ConfigureSwagger(services);
        }

        private void ConfigureClient(IServiceCollection services)
        {
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
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API сервиса менеджера агента сбора метрик",
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
            
            // Включение middleware в пайплайн для обработки Swagger запросов.
            app.UseSwagger(); 
            // включение middleware для генерации swagger-ui
            // указываем Swagger JSON эндпоинт (куда обращаться за сгенерированной спецификацией
            // по которой будет построен UI).
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса менеджера агента сбора метрик");
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