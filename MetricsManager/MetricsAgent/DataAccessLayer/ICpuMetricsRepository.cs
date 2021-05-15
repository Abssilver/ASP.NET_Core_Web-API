using MetricsAgent.Metrics;

namespace MetricsAgent.DataAccessLayer
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
    }
}