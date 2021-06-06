using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.Client.Interfaces
{
    public interface ICpuMetricsClient
    {
        GetByPeriodCpuMetricsClientResponse GetMetricsFromAgent(GetCpuMetricsFromAgentRequest request);

        GetByPeriodCpuMetricsClientResponse GetMetricsFromAllCluster(GetAllCpuMetricsRequest request);
    }
}