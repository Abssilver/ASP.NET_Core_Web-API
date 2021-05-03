namespace MetricsAgent.DataAccessLayer
{
    public class DatabaseSettingsProvider: IDatabaseSettingsProvider
    {
        private readonly string _connectionString;

        public DatabaseSettingsProvider()
        {
            _connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
        }

        public string GetConnectionString() => _connectionString;
    }
}