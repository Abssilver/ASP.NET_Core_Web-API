using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;

        public HddMetricsController(ILogger<HddMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
        }
        
        [HttpGet("agent/{agentId}/left")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId)
        {
            _logger.LogInformation($"Агент: {agentId}");
            return Ok();
        }
        
        
        [HttpGet("cluster/left")]
        public IActionResult GetMetricsFromAllCluster()
        {
            _logger.LogInformation($"Запрос общих данных");
            return Ok();
        }
    }
}
