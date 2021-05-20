using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class HddMetricJob : IJob
    {
        private readonly IHddMetricsRepository _repository;
        private readonly PerformanceCounter _counter;
        
        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter(
                "PhysicalDisk", "% Disk Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //disk activity %
            var metrics = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;
            
            _repository.Create(new HddMetric { Time = time, Value = metrics });
            
            return Task.CompletedTask;
        }

    }
}