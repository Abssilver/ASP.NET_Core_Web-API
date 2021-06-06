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
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public DotNetMetricsController(
            IDotNetMetricsManagerRepository managerRepository, 
            ILogger<DotNetMetricsController> logger, 
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");

        }

        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени по определенному агенту
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/dotnet/agent/1/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="agentId">Id зарегистрированного агента</param>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public GetByPeriodDotNetMetricsApiResponse GetMetricsFromAgent(
            [FromRoute] int agentId,
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Агент: {agentId}, From:{fromTime}, To:{toTime}");
            
            var metrics = _managerRepository.GetByTimePeriodFromAgent(fromTime, toTime, agentId);

            var response = new GetByPeriodDotNetMetricsApiResponse
            {
                Metrics = new List<ApiDotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<ApiDotNetMetricDto>(metric));
            }

            return response;
        }
        
        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени по всем агентам
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/dotnet/cluster/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public GetByPeriodDotNetMetricsApiResponse GetMetricsFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Общие данные From:{fromTime}, To:{toTime}");
            
            var metrics = _managerRepository.GetByTimePeriod(fromTime, toTime);

            var response = new GetByPeriodDotNetMetricsApiResponse
            {
                Metrics = new List<ApiDotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<ApiDotNetMetricDto>(metric));
            }

            return response;
        }
    }
}