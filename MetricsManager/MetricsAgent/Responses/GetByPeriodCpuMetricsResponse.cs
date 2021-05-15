using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class GetByPeriodCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }
}