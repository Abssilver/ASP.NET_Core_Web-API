using System.Collections.Generic;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.Responses
{
    public class GetByPeriodCpuMetricsApiResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }
}