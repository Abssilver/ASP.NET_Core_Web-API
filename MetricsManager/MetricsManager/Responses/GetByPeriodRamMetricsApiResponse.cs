using System.Collections.Generic;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.Responses
{
    public class GetByPeriodRamMetricsApiResponse
    {
        public List<ApiRamMetricDto> Metrics { get; set; }
    }
}