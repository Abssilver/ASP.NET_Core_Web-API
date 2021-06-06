using System;
using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.DataLayer.Interfaces
{
    public interface ICpuMetricModel
    {
        public Queue<CpuMetricClientDto> Metrics { get; set; }
        public DateTimeOffset LastAddedTime { get; }
        void AddMetrics(List<CpuMetricClientDto> recievedMetrics);
        
        public event Action OnMetricsValueChange;
    }
}