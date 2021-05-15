using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class GetByPeriodHddMetricsResponse
    {
        public List<HddMetricDto> Metrics { get; set; }
    }
}