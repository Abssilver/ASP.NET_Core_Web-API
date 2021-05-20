using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.DataAccessLayer.Repositories
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly string _connectionString;
        
        public RamMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }
        
        public void Create(RamMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO rammetrics(value, time) VALUES(@value, @time)", 
                new { 
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }
        
        public IList<RamMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<RamMetric>(string.Format(
                    "SELECT Id, Time, Value FROM rammetrics WHERE time BETWEEN {0} AND {1}", 
                    from.ToUnixTimeSeconds(), 
                    to.ToUnixTimeSeconds()))
                .ToList();
        }
    }
}