using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _counter;
        
        public CpuMetricJob(ICpuMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_counter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new CpuMetric { Time = time, Value = cpuUsageInPercents });
            
            return Task.CompletedTask;
        }

    }
}