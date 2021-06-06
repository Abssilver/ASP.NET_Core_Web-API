using System;
using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.DataLayer.Interfaces
{
    public interface INetworkMetricModel
    {
        public Queue<NetworkMetricClientDto> Metrics { get; set; }
        public DateTimeOffset LastAddedTime { get; }
        
        void AddMetrics(List<NetworkMetricClientDto> recievedMetrics);
        public event Action OnMetricsValueChange;
    }
}