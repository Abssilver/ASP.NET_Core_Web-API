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
    public class DotNetMetricJob : IJob
    {
        private readonly IDotNetMetricsManagerRepository _managerRepository;
        private readonly IDotNetMetricsAgentClient _agentClient;
        private readonly IAgentInfoRepository _agentRepository;

        public DotNetMetricJob(
            IDotNetMetricsManagerRepository managerRepository,
            IDotNetMetricsAgentClient client,
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
                var metrics = _agentClient.GetDotNetMetrics(new DotNetMetricApiGetRequest
                    {
                        FromTime = _managerRepository.GetLastRecordDate(),
                        //TODO: косяк со временем, надо брать большее значение, чем текущая дата
                        //ToTime = DateTimeOffset.UtcNow
                        ToTime = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86_400),
                        ClientBaseAddress = info.Url.ToString(),
                    }
                );

                metrics?.Metrics.ForEach(metric =>
                    {
                        _managerRepository.Create(new ApiDotNetMetric
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