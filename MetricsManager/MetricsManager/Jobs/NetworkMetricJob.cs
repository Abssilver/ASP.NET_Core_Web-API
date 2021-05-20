using System;
using System.Threading.Tasks;
using AutoMapper.Internal;
using MetricsManager.Client.Interfaces;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Models;
using MetricsManager.Requests;
using Quartz;

namespace MetricsManager.Jobs
{
    [DisallowConcurrentExecution]
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricsManagerRepository _managerRepository;
        private readonly INetworkMetricsAgentClient _agentClient;
        private readonly IAgentInfoRepository _agentRepository;
        
        public NetworkMetricJob(
            INetworkMetricsManagerRepository managerRepository,
            INetworkMetricsAgentClient client,
            IAgentInfoRepository agentsRepository)
        {
            _managerRepository = managerRepository;
            _agentClient = client;
            _agentRepository = agentsRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var agents = _agentRepository.GetAgents();

            agents.ForAll(info =>
            {
                var metrics = _agentClient.GetNetworkMetrics(new NetworkMetricApiGetRequest
                    {
                        FromTime = _managerRepository.GetLastRecordDate(),
                        ToTime = DateTimeOffset.UtcNow,
                        ClientBaseAddress = info.Url.ToString(),
                    }
                );

                metrics?.Metrics.ForEach(metric =>
                    {
                        _managerRepository.Create(new NetworkMetric
                        {
                            Time = metric.Time,
                            Value = metric.Value,
                            AgentId = info.Id,
                        });
                    }
                );
            });

            return Task.CompletedTask;
        }
    }
}