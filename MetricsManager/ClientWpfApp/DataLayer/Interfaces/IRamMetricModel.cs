using System;
using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.DataLayer.Interfaces
{
    public interface IRamMetricModel
    {
        public Queue<RamMetricClientDto> Metrics { get; set; }
        public DateTimeOffset LastAddedTime { get; }
        
        void AddMetrics(List<RamMetricClientDto> recievedMetrics);
        public event Action OnMetricsValueChange;
    }
}