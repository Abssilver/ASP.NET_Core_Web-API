using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.Metrics;

namespace MetricsAgent.DataAccessLayer
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
            connection.Open();
            // создаем команду
            using var cmd = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)"
            };
            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);

            // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
            // через свойство
            cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
            // подготовка команды к выполнению
            cmd.Prepare();
            // выполнение команды
            cmd.ExecuteNonQuery();
        }
        
        public IList<CpuMetric> GetByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            
            cmd.CommandText = string.Format("SELECT * FROM cpumetrics WHERE time BETWEEN {0} AND {1}",
                from.ToUnixTimeSeconds(),
                to.ToUnixTimeSeconds());

            var returnList = new List<CpuMetric>();

            using SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                returnList.Add(new CpuMetric
                {
                    Id = reader.GetInt32(0),
                    Value = reader.GetInt32(1),
                    Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(2))//.LocalDateTime 
                });
            }

            return returnList;
        }

        //TODO: методы из методички (из-за неиспользования могут быть удалены)
        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = "DELETE FROM cpumetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(CpuMetric item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }
        
        public IList<CpuMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM cpumetrics";

            var returnList = new List<CpuMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64(2)) 
                    });
                }
            }

            return returnList;
        }

        public CpuMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM cpumetrics WHERE id=@id";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64(2)) 
                    };
                }

                // не нашлось запись по идентификатору, не делаем ничего
                return null;
            }
        }
        
    }
}