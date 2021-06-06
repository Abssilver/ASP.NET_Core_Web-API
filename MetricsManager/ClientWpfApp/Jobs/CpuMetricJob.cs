using System;
using System.Threading.Tasks;
using MetricsManagerClient.Client.Interfaces;
using MetricsManagerClient.DataLayer.Interfaces;
using MetricsManagerClient.Requests;
using Quartz;

namespace MetricsManagerClient.Jobs
{
    [DisallowConcurrentExecution]
    public class CpuMetricJob : IJob
    {
        private readonly ICpuMetricModel _model;
        private readonly ICpuMetricsClient _client;
        private readonly IAppModel _appModel;

        public CpuMetricJob(
            ICpuMetricModel model,
            ICpuMetricsClient client,
            IAppModel appModel)
        {
            _model = model;
            _client = client;
            _appModel = appModel;
        }

        public Task Execute(IJobExecutionContext context)
        {
            if (!_appModel.IsFollowAgent) 
                return Task.CompletedTask;

            var metrics = _client.GetMetricsFromAllCluster(new GetAllCpuMetricsRequest
            {
                FromTime = _model.LastAddedTime,
                ToTime = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86_400)
            });

            _model.AddMetrics(metrics.Metrics);

            return Task.CompletedTask;
        }
    }
}