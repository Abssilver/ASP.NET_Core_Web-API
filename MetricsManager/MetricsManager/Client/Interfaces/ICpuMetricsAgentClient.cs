using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client.Interfaces
{
    public interface ICpuMetricsAgentClient
    {
        GetByPeriodCpuMetricsApiResponse GetCpuMetrics(CpuMetricApiGetRequest request);
    }
}