using System.Collections.Generic;
using MetricsAgent.Responses.DataTransferObjects;

namespace MetricsAgent.Responses
{
    public class GetByPeriodRamMetricsResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }
}