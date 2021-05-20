using Core.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.DataAccessLayer.Interfaces
{
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {
    }
}