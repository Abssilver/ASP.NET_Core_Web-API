using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client.Interfaces
{
    public interface IRamMetricsAgentClient
    {
        GetByPeriodRamMetricsApiResponse GetRamMetrics(RamMetricApiGetRequest request);
    }
}