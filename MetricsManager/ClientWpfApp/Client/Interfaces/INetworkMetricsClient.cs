using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.Client.Interfaces
{
    public interface INetworkMetricsClient
    {
        GetByPeriodNetworkMetricsClientResponse GetMetricsFromAgent(GetNetworkMetricsFromAgentRequest request);

        GetByPeriodNetworkMetricsClientResponse GetMetricsFromAllCluster(GetAllNetworkMetricsRequest request);
    }
}