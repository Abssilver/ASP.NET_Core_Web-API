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
    public class HddMetricsRepository : IHddMetricsManagerRepository
    {
        private readonly string _connectionString;
        
        public HddMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }
        
        public void Create(ApiHddMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO hddmetrics(value, time, Agent_id) VALUES(@value, @time, @Agent_id)",
                new { 
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    Agent_id = item.AgentId,
                });
        }
        
        public IList<ApiHddMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<ApiHddMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM hddmetrics WHERE time BETWEEN @fromTime AND @ToTime",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<ApiHddMetric> GetByTimePeriodFromAgent(DateTimeOffset from, DateTimeOffset to, int agentId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<ApiHddMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM hddmetrics WHERE (time BETWEEN @fromTime AND @ToTime) AND (Agent_Id = @agent_Id)",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds(),
                        agent_Id = agentId,
                    })
                .ToList();
        }
        
        public DateTimeOffset GetLastRecordDate(int agentId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            
            var response = connection.QuerySingleOrDefault<GetLastTimeResponse>(
                "SELECT MAX(Time) AS Time FROM hddmetrics WHERE Agent_Id=@id",
                new
                {
                    id = agentId
                }); 
            return DateTimeOffset
                .FromUnixTimeSeconds(response.Time);
        }
    }
}