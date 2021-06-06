using System;
using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.DataLayer.Interfaces
{
    public interface IDotNetMetricModel
    {
        public Queue<DotNetMetricClientDto> Metrics { get; set; }
        public DateTimeOffset LastAddedTime { get; }
        void AddMetrics(List<DotNetMetricClientDto> recievedMetrics);
        public event Action OnMetricsValueChange;
    }
}