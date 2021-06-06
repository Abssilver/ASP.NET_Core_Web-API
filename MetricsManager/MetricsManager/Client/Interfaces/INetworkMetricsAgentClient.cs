using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client.Interfaces
{
    public interface INetworkMetricsAgentClient
    {
        GetByPeriodNetworkMetricsApiResponse GetNetworkMetrics(NetworkMetricApiGetRequest request);
    }
}