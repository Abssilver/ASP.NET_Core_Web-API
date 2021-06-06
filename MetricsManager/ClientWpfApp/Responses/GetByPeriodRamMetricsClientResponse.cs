using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.Responses
{
    public class GetByPeriodRamMetricsClientResponse
    {
        public List<RamMetricClientDto> Metrics { get; set; }
    }
}