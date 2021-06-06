using System.Collections.Generic;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.Responses
{
    public class GetByPeriodDotNetMetricsApiResponse
    {
        public List<ApiDotNetMetricDto> Metrics { get; set; }
    }
}