using MetricsManager.Model;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentsModel _agentsModel;

        public AgentsController(AgentsModel agentsModel)
        {
            _agentsModel = agentsModel;
        }

        
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            return Ok();
        }
        
        
        [HttpDelete("unregister")]
        public IActionResult UnregisterAgent([FromBody] AgentInfo agentInfo)
        {
            return Ok();
        }
        
        
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }
        
        
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }
        
        
        [HttpGet("get_agents")]
        public IActionResult GetRegisterAgents()
        {
            return Ok(_agentsModel.GetAgentsInfo());
        }
    }
}