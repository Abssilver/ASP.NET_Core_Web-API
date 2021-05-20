using System.Collections.Generic;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.Responses
{
    public class GetByPeriodRamMetricsApiResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }
}