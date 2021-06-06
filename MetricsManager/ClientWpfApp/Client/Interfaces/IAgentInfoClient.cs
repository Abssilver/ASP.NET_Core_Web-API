using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.Client.Interfaces
{
    public interface IAgentInfoClient
    {
        void RegisterAgent(AgentInfoRegisterRequest request);
        
        void UnregisterAgent(AgentInfoUnregisterRequest request);
        
        GetAgentsInfoClientResponse GetRegisterAgents();
    }
}