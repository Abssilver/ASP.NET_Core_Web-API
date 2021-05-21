namespace MetricsManager.Client.Interfaces
{
    public interface IMetricsAgentClient: 
        ICpuMetricsAgentClient,
        IDotNetMetricsAgentClient,
        IHddMetricsAgentClient,
        INetworkMetricsAgentClient,
        IRamMetricsAgentClient
    { }
}