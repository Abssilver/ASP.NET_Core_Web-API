using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.Metrics;

namespace MetricsAgent.DataAccessLayer
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
            connection.Open();
            
            using var cmd = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO hddmetrics(value, time) VALUES(@value, @time)"
            };
            
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
            
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        public IList<HddMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection)
            {
                CommandText = string.Format("SELECT * FROM hddmetrics WHERE time BETWEEN {0} AND {1}",
                    from.ToUnixTimeSeconds(),
                    to.ToUnixTimeSeconds())
            };


            var returnList = new List<HddMetric>();

            using SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                returnList.Add(new HddMetric
                {
                    Id = reader.GetInt32(0),
                    Value = reader.GetInt32(1),
                    Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(2)).LocalDateTime 
                });
            }

            return returnList;
        }
    }
}