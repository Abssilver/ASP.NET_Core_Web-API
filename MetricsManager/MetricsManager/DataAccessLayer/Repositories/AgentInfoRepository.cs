using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Models;

namespace MetricsManager.DataAccessLayer.Repositories
{
    public class AgentInfoRepository : IAgentInfoRepository
    {
        private readonly string _connectionString;
        
        public AgentInfoRepository(IDatabaseSettingsProvider dbProvider)
        {
            _connectionString = dbProvider.GetConnectionString();
        }

        public void Create(AgentInfo item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("INSERT INTO agents(url) VALUES(@agentUrl)", 
                new {
                    agentUrl = item.Url,
                });
        }
        
        public void Delete(string url)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Execute("DELETE FROM agents WHERE url=@agentUrl",
                new
                {
                    agentUrl = url,
                });
        }
        
        public IList<AgentInfo> GetAgents()
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection
                .Query<AgentInfo>("SELECT id, url FROM agents")
                .ToList();
        }
    }
}