using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class GetByPeriodDotNetMetricsResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }
    }
}