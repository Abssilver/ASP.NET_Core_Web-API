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
    public class NetworkMetricsRepository : INetworkMetricsManagerRepository
    {
        private readonly string _connectionString;
        
        public NetworkMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }
        
        public void Create(NetworkMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO networkmetrics(value, time, Agent_id) VALUES(@value, @time, @Agent_id)", 
                new { 
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    Agent_id = item.AgentId,
                });
        }

        public IList<NetworkMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<NetworkMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM networkmetrics WHERE time BETWEEN @fromTime AND @ToTime",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds()
                    })
                .ToList();
        }
        
        public IList<NetworkMetric> GetByTimePeriodFromAgent(DateTimeOffset from, DateTimeOffset to, int agentId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<NetworkMetric>(
                    "SELECT Id, Time, Value, Agent_Id AS AgentId FROM networkmetrics WHERE (time BETWEEN @fromTime AND @ToTime) AND (Agent_Id = @agent_Id)",
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
                "SELECT MAX(Time) AS Time FROM networkmetrics"); 
            return DateTimeOffset
                .FromUnixTimeSeconds(response.Time);
        }
    }
}