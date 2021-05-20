using System.Collections.Generic;
using MetricsAgent.Responses.DataTransferObjects;

namespace MetricsAgent.Responses
{
    public class GetByPeriodCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }
}