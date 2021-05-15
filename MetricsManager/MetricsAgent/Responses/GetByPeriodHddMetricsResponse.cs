using System.Collections.Generic;
using MetricsAgent.Responses.DataTransferObjects;

namespace MetricsAgent.Responses
{
    public class GetByPeriodHddMetricsResponse
    {
        public List<HddMetricDto> Metrics { get; set; }
    }
}