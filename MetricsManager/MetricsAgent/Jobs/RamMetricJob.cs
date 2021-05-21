using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class RamMetricJob : IJob
    {
        private readonly IRamMetricsRepository _repository;
        private readonly PerformanceCounter _counter;
        
        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //use of allocated memory %
            var metrics = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;
            
            _repository.Create(new RamMetric { Time = time, Value = metrics });
            
            return Task.CompletedTask;
        }

    }
}