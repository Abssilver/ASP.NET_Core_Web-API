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
    public class RamMetricJob : IJob
    {
        private readonly IRamMetricsManagerRepository _managerRepository;
        private readonly IRamMetricsAgentClient _agentClient;
        private readonly IAgentInfoRepository _agentRepository;
        
        public RamMetricJob(
            IRamMetricsManagerRepository managerRepository,
            IRamMetricsAgentClient client,
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
                var metrics = _agentClient.GetRamMetrics(new RamMetricApiGetRequest
                    {
                        FromTime = _managerRepository.GetLastRecordDate(),
                        ToTime = DateTimeOffset.UtcNow,
                        ClientBaseAddress = info.Url.ToString(),
                    }
                );

                metrics?.Metrics.ForEach(metric =>
                    {
                        _managerRepository.Create(new RamMetric
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