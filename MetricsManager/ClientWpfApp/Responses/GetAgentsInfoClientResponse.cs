using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.Responses
{
    public class GetAgentsInfoClientResponse
    {
        public List<AgentInfoClientDto> Agents { get; set; }
    }
}