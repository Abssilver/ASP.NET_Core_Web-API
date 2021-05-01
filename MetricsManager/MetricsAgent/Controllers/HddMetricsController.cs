using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        [HttpGet("left")]
        public IActionResult GetRemainingFreeDiskSpaceMetrics()
        {
            return Ok();
        }
    }
}
