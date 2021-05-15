namespace MetricsAgent.DataAccessLayer
{
    public interface IDatabaseSettingsProvider
    {
        string GetConnectionString();
    }
}