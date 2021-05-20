using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.DataAccessLayer.Repositories
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly string _connectionString;
        
        public HddMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }
        
        public void Create(HddMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)", 
                new { 
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }
        
        public IList<HddMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<HddMetric>(string.Format(
                    "SELECT Id, Time, Value FROM hddmetrics WHERE time BETWEEN {0} AND {1}", 
                    from.ToUnixTimeSeconds(), 
                    to.ToUnixTimeSeconds()))
                .ToList();
        }
    }
}