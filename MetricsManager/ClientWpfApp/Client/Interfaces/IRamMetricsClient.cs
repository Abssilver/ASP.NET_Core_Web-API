using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.Client.Interfaces
{
    public interface IRamMetricsClient
    {
        GetByPeriodRamMetricsClientResponse GetMetricsFromAgent(GetRamMetricsFromAgentRequest request);

        GetByPeriodRamMetricsClientResponse GetMetricsFromAllCluster(GetAllRamMetricsRequest request);
    }
}