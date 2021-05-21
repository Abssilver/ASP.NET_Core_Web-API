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
    public class CpuMetricJob : IJob
    {
        private readonly ICpuMetricsManagerRepository _managerRepository;
        private readonly ICpuMetricsAgentClient _agentClient;
        private readonly IAgentInfoRepository _agentRepository;

        public CpuMetricJob(
            ICpuMetricsManagerRepository managerRepository, 
            ICpuMetricsAgentClient client, 
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
                var metrics = _agentClient.GetCpuMetrics(new CpuMetricApiGetRequest
                    {
                        FromTime =  _managerRepository.GetLastRecordDate(), 
                        //TODO: косяк со временем, надо брать большее значение, чем текущая дата
                        //ToTime = DateTimeOffset.UtcNow
                        ToTime = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86_400), 
                        ClientBaseAddress = info.Url.ToString(),
                    }
                );
                
                metrics?.Metrics.ForEach(metric =>
                    {
                        _managerRepository.Create(new ApiCpuMetric
                        {
                            Time = DateTimeOffset.UtcNow,
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