using System;
using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.DataLayer.Interfaces
{
    public interface IHddMetricModel
    {
        public Queue<HddMetricClientDto> Metrics { get; set; }
        public DateTimeOffset LastAddedTime { get; }
        
        void AddMetrics(List<HddMetricClientDto> recievedMetrics);
        public event Action OnMetricsValueChange;
    }
}