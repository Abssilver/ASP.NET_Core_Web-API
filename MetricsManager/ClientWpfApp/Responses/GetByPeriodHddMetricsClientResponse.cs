using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.Responses
{
    public class GetByPeriodHddMetricsClientResponse
    {
        public List<HddMetricClientDto> Metrics { get; set; }
    }
}