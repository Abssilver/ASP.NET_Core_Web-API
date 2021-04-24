using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        [HttpGet("agent/{agentId}/available")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId)
        {
            return Ok();
        }
        

        [HttpGet("cluster/available")]
        public IActionResult GetMetricsFromAllCluster()
        {
            return Ok();
        }
    }
}