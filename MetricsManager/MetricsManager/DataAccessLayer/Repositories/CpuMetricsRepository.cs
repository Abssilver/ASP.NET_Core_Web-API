using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Models;
using MetricsManager.Responses;

namespace MetricsManager.DataAccessLayer.Repositories
{
    public class CpuMetricsRepository : ICpuMetricsManagerRepository
    {
        private readonly string _connectionString;

        public CpuMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }

        public void Create(ApiCpuMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO cpumetrics(value, time, Agent_id) VALUES(@value, @time, @Agent_id)", 
                new { 
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    Agent_id = item.AgentId,
                });
        }
        
        public IList<ApiCpuMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<ApiCpuMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM cpumetrics WHERE time BETWEEN @fromTime AND @ToTime",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<ApiCpuMetric> GetByTimePeriodFromAgent(DateTimeOffset from, DateTimeOffset to, int agentId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<ApiCpuMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM cpumetrics WHERE (time BETWEEN @fromTime AND @ToTime) AND (Agent_Id = @agent_Id)",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds(),
                        agent_Id = agentId,
                    })
                .ToList();
        }

        public DateTimeOffset GetLastRecordDate()
        {
            using var connection = new SQLiteConnection(_connectionString);
            
            var response = connection.QuerySingleOrDefault<GetLastTimeResponse>(
                "SELECT MAX(Time) AS Time FROM cpumetrics"); 
            return DateTimeOffset
                .FromUnixTimeSeconds(response.Time);
        }

        //TODO: методы из методички (из-за неиспользования могут быть удалены)
        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("DELETE FROM cpumetrics WHERE id=@id",
                new
                {
                    id = id
                });
        }

        public void Update(ApiCpuMetric item)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time,
                        id = item.Id
                    });
            }
        }
        
        public IList<ApiCpuMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_connectionString);
            // читаем при помощи Query и в шаблон подставляем тип данных
            // объект которого Dapper сам и заполнит его поля
            // в соответсвии с названиями колонок
            return connection.Query<ApiCpuMetric>("SELECT Id, Time, Value FROM cpumetrics").ToList();
        }

        public ApiCpuMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection.QuerySingle<ApiCpuMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id=@id",
                new {id = id});
        }
    }
}