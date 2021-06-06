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
        /// <summary>
        /// Производит регистрацию агента (Создание записи в БД)
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST url:port/api/agents/register
        /// 
        ///     Body: { "Address": "url:port" }
        ///
        /// </remarks>
        /// <param name="request">Данные запроса по агенту, который подлежит регистрации</param>
        /// <returns>None</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
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
        
        /// <summary>
        /// Производит снятие регистрации агента (удаление из БД)
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE url:port/api/agents/unregister
        ///
        ///     Body: { "Address": "url:port" }
        /// 
        /// </remarks>
        /// <param name="request">Данные запроса по агенту, который подлежит снятию с регистрации</param>
        /// <returns>None</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpDelete("unregister")]
        public IActionResult UnregisterAgent([FromBody] AgentInfoUnregisterRequest request)
        {
            _logger.LogInformation(
                $"Снятие регистрации агента address:{request.Address}");
            
            _managerRepository.Delete(request.Address);
            
            return Ok();
        }
        
        /// <summary>
        /// Недокумментированный метод
        /// </summary>
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"Активация агента id:{agentId}");
            return Ok();
        }
        
        /// <summary>
        /// Недокумментированный метод
        /// </summary>
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"Деактивация агента id:{agentId}");
            return Ok();
        }
        
        /// <summary>
        /// Получает список всех зарегистрированных агентов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET url:port/api/agents/get_agents
        /// 
        /// </remarks>
        /// <returns>Список зарегистрированных агентов</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("get_agents")]
        public GetAgentsInfoResponse GetRegisterAgents()
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

            return response;
        }
    }
}