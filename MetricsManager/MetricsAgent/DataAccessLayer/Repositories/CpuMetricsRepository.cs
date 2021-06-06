using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.DataAccessLayer.Repositories
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly string _connectionString;

        public CpuMetricsRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }

        public void Create(CpuMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            //  запрос на вставку данных с плейсхолдерами для параметров
            connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)", 
                // анонимный объект с параметрами запроса
                new { 
                    // value подставится на место "@value" в строке запроса
                    // значение запишется из поля Value объекта item
                    value = item.Value,
                    
                    // записываем в поле time количество секунд
                    time = item.Time.ToUnixTimeSeconds()
                });
        }
        
        public IList<CpuMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<CpuMetric>(
                    "SELECT Id, Time, Value FROM cpumetrics WHERE time BETWEEN @fromTime AND @ToTime",
                    new
                    {
                        fromTime = from.ToUnixTimeSeconds(),
                        ToTime = to.ToUnixTimeSeconds()
                    })
                .ToList();
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

        public void Update(CpuMetric item)
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
        
        public IList<CpuMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_connectionString);
            // читаем при помощи Query и в шаблон подставляем тип данных
            // объект которого Dapper сам и заполнит его поля
            // в соответсвии с названиями колонок
            return connection.Query<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics").ToList();
        }

        public CpuMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection.QuerySingle<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id=@id",
                new {id = id});
        }
    }
}