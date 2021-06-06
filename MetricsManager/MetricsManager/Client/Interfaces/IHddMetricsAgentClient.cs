using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client.Interfaces
{
    public interface IHddMetricsAgentClient
    {
        GetByPeriodHddMetricsApiResponse GetHddMetrics(HddMetricsApiGetRequest request);
    }
}