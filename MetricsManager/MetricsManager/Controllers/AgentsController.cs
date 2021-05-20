using System.Collections.Generic;
using AutoMapper;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;
using MetricsManager.Responses.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private readonly IAgentInfoRepository _managerRepository;
        private readonly IMapper _mapper;

        public AgentsController(
            IAgentInfoRepository managerRepository, 
            ILogger<AgentsController> logger, 
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в AgentsController");
        }


        //http://localhost:51685/api/agents/register
        //Body: { "Address": "http://localhost:51684" }
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfoRegisterRequest request)
        {
            
            _logger.LogInformation(
                $"Регистрация агента address:{request.Address}");
            
            _managerRepository.Create(new AgentInfo
            {
                Url = request.Address,
            });
            
            return Ok();
        }
        
        //http://localhost:51685/api/agents/unregister
        //Body: { "Address": "http://localhost:51684" }
        [HttpDelete("unregister")]
        public IActionResult UnregisterAgent([FromBody] AgentInfoUnregisterRequest request)
        {
            _logger.LogInformation(
                $"Снятие регистрации агента address:{request.Address}");
            
            _managerRepository.Delete(request.Address);
            
            return Ok();
        }
        
        
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"Активация агента id:{agentId}");
            return Ok();
        }
        
        
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"Деактивация агента id:{agentId}");
            return Ok();
        }
        
        
        [HttpGet("get_agents")]
        public IActionResult GetRegisterAgents()
        {
            _logger.LogInformation($"Запрос данных об агентах");
            
            var agents = _managerRepository.GetAgents();

            var response = new GetAgentsInfoResponse
            {
                Agents = new List<AgentInfoDto>()
            };

            foreach (var agent in agents)
            {
                response.Agents.Add(_mapper.Map<AgentInfoDto>(agent));
            }

            return Ok(response);
        }
    }
}