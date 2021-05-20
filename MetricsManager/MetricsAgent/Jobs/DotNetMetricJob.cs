using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private readonly IDotNetMetricsRepository _repository;
        private readonly PerformanceCounter _counter;
        
        public DotNetMetricJob(IDotNetMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter(
                ".NET CLR Memory", "# Bytes in all Heaps", "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //bytes in all heaps
            var metrics = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;
            
            _repository.Create(new DotNetMetric { Time = time, Value = metrics });
            
            return Task.CompletedTask;
        }

    }
}