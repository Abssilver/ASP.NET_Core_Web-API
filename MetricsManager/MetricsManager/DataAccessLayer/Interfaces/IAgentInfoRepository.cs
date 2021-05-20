
using System.Collections.Generic;
using MetricsManager.DataAccessLayer.Models;

namespace MetricsManager.DataAccessLayer.Interfaces
{
    public interface IAgentInfoRepository
    {
        void Create(AgentInfo item);
        void Delete(string url);
        IList<AgentInfo> GetAgents();
    }
}