using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.Client.Interfaces
{
    public interface IHddMetricsClient
    {
        GetByPeriodHddMetricsClientResponse GetMetricsFromAgent(GetHddMetricsFromAgentRequest request);

        GetByPeriodHddMetricsClientResponse GetMetricsFromAllCluster(GetAllHddMetricsRequest request);
    }
}