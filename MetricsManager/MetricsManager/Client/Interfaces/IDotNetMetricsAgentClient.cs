using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client.Interfaces
{
    public interface IDotNetMetricsAgentClient
    {
        GetByPeriodDotNetMetricsApiResponse GetDotNetMetrics(DotNetMetricApiGetRequest request);
    }
}