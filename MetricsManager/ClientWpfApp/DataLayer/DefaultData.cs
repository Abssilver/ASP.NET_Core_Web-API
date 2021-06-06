using System;
using System.Collections.Generic;
using System.Linq;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.DataLayer
{
    public static class DefaultData
    {
        public static readonly List<CpuMetricClientDto> CpuMetric = Enumerable.Range(1, 20)
            .Select(i => new CpuMetricClientDto
            {
                Id = 0,
                Value = 0, 
                Time = DateTimeOffset.UtcNow,
            }).ToList();
        
        public static readonly List<DotNetMetricClientDto> DotNetMetric = Enumerable.Range(1, 20)
            .Select(i => new DotNetMetricClientDto
            {
                Id = 0,
                Value = 0, 
                Time = DateTimeOffset.UtcNow,
            }).ToList();
        
        public static readonly List<HddMetricClientDto> HddMetric = Enumerable.Range(1, 20)
            .Select(i => new HddMetricClientDto
            {
                Id = 0,
                Value = 0, 
                Time = DateTimeOffset.UtcNow,
            }).ToList();
        
        public static readonly List<NetworkMetricClientDto> NetworkMetric = Enumerable.Range(1, 20)
            .Select(i => new NetworkMetricClientDto
            {
                Id = 0,
                Value = 0, 
                Time = DateTimeOffset.UtcNow,
            }).ToList();
        
        public static readonly List<RamMetricClientDto> RamMetric = Enumerable.Range(1, 20)
            .Select(i => new RamMetricClientDto
            {
                Id = 0,
                Value = 0, 
                Time = DateTimeOffset.UtcNow,
            }).ToList();
    }
}