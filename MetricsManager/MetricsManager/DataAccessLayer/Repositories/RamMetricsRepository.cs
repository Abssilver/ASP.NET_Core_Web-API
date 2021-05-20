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
    public class RamMetricsRepository : IRamMetricsManagerRepository
    {
        private readonly string _connectionString;
        
        public RamMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }
        
        public void Create(RamMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO rammetrics(value, time, Agent_id) VALUES(@value, @time, @Agent_id)",
                new { 
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    Agent_id = item.AgentId,
                });
        }

        public IList<RamMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<RamMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM rammetrics WHERE time BETWEEN @fromTime AND @ToTime",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<RamMetric> GetByTimePeriodFromAgent(DateTimeOffset from, DateTimeOffset to, int agentId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<RamMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM rammetrics WHERE (time BETWEEN @fromTime AND @ToTime) AND (Agent_Id = @agent_Id)",
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
                "SELECT MAX(Time) AS Time FROM rammetrics"); 
            return DateTimeOffset
                .FromUnixTimeSeconds(response.Time);
        }
    }
}