using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class GetByPeriodRamMetricsResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }
}