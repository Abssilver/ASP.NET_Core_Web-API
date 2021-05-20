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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHddMetricsManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public HddMetricsController(
            IHddMetricsManagerRepository managerRepository, 
            ILogger<HddMetricsController> logger, 
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
        }

        /// <summary>
        /// Получает метрики HDD на заданном диапазоне времени по определенному агенту
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/hdd/agent/1/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
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

            var response = new GetByPeriodHddMetricsApiResponse
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            return Ok(response);
        }
        
        /// <summary>
        /// Получает метрики HDD на заданном диапазоне времени по всем агентам
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/hdd/cluster/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
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

            var response = new GetByPeriodHddMetricsApiResponse
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
