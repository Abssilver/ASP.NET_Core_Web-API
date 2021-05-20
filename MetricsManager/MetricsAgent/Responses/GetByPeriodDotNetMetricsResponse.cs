using System.Collections.Generic;
using MetricsAgent.Responses.DataTransferObjects;

namespace MetricsAgent.Responses
{
    public class GetByPeriodDotNetMetricsResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }
    }
}