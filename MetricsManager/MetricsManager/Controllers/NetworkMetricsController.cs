using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.Responses;
using MetricsManager.Responses.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly INetworkMetricsManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public NetworkMetricsController(
            INetworkMetricsManagerRepository managerRepository, 
            ILogger<NetworkMetricsController> logger, 
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в NetworkMetricsController");

        }

        /// <summary>
        /// Получает метрики Network на заданном диапазоне времени по определенному агенту
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/network/agent/1/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="agentId">Id зарегистрированного агента</param>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId,
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Агент: {agentId}, From:{fromTime}, To:{toTime}");
            
            var metrics = _managerRepository.GetByTimePeriodFromAgent(fromTime, toTime, agentId);

            var response = new GetByPeriodNetworkMetricsApiResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            return Ok(response);
        }

        /// <summary>
        /// Получает метрики Network на заданном диапазоне времени по всем агентам
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/network/cluster/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Общие данные From:{fromTime}, To:{toTime}");
            
            var metrics = _managerRepository.GetByTimePeriod(fromTime, toTime);

            var response = new GetByPeriodNetworkMetricsApiResponse
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}