using System;
using System.Data.SQLite;
using MetricsAgent.DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSqlLiteConnection(services);
            services.AddScoped<ICpuMetricsRepository,CpuMetricsRepository>();
            services.AddScoped<IDotNetMetricsRepository,DotNetMetricsRepository>();
            services.AddScoped<IHddMetricsRepository,HddMetricsRepository>();
            services.AddScoped<INetworkMetricsRepository,NetworkMetricsRepository>();
            services.AddScoped<IRamMetricsRepository,RamMetricsRepository>();
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
            CreateFakeData("cpumetrics", connection);
            CreateFakeData("dotnetmetrics", connection);
            CreateFakeData("hddmetrics", connection);
            CreateFakeData("networkmetrics", connection);
            CreateFakeData("rammetrics", connection);
            /*
            using (var command = new SQLiteCommand(connection))
            {
                // задаем новый текст команды для выполнения
                // удаляем таблицу с метриками если она существует в базе данных
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                // отправляем запрос в базу данных
                command.ExecuteNonQuery();

                
                command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
                command.ExecuteNonQuery();
            }
            */
        }

        private void CreateFakeData(string tableName, SQLiteConnection connection)
        {
            var rnd = new Random();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE {tableName}(id INTEGER PRIMARY KEY, value INTEGER, time INTEGER)";
                command.ExecuteNonQuery();

                //данные за последние 10 дней
                //перед запуском нужно выставить текущее число +1 и 10 - 1
                for (int i = 0; i < 10; i++)
                {

                    command.CommandText = $"INSERT INTO {tableName}(value, time) VALUES(@value, @time)";

                    command.Parameters.AddWithValue(
                        "@value",
                        rnd.Next(0, 100));
                    command.Parameters.AddWithValue(
                        "@time",
                        DateTimeOffset.Now.ToUnixTimeSeconds() - 86_400 * i);
                    
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}