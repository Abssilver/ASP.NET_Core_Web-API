using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.Responses
{
    public class GetByPeriodCpuMetricsClientResponse
    {
        public List<CpuMetricClientDto> Metrics { get; set; }
    }
}