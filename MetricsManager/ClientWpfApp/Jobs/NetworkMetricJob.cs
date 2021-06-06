using System;
using System.Threading.Tasks;
using MetricsManagerClient.Client.Interfaces;
using MetricsManagerClient.DataLayer.Interfaces;
using MetricsManagerClient.Requests;
using Quartz;

namespace MetricsManagerClient.Jobs
{
    [DisallowConcurrentExecution]
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricModel _model;
        private readonly INetworkMetricsClient _client;
        private readonly IAppModel _appModel;

        public NetworkMetricJob(
            INetworkMetricModel model,
            INetworkMetricsClient client,
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

            var metrics = _client.GetMetricsFromAllCluster(new GetAllNetworkMetricsRequest
            {
                FromTime = _model.LastAddedTime,
                ToTime = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86_400)
            });

            _model.AddMetrics(metrics.Metrics);

            return Task.CompletedTask;
        }
    }
}