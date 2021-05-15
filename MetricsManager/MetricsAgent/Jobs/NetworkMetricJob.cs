using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricsRepository _repository;
        private readonly PerformanceCounter _counter;
        
        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter(
                "Network Adapter", "Bytes Total/sec", "Microsoft Kernel Debug Network Adapter");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //total bytes/s
            var metrics = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;
            
            _repository.Create(new NetworkMetric { Time = time, Value = metrics });
            
            return Task.CompletedTask;
        }

    }
}