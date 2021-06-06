using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.Client.Interfaces
{
    public interface IDotNetMetricsClient
    {
        GetByPeriodDotNetMetricsClientResponse GetMetricsFromAgent(GetDotNetMetricsFromAgentRequest request);

        GetByPeriodDotNetMetricsClientResponse GetMetricsFromAllCluster(GetAllDotNetMetricsRequest request);
    }
}