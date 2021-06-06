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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public CpuMetricsController(
            ICpuMetricsManagerRepository managerRepository, 
            ILogger<CpuMetricsController> logger, 
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogInformation(1, "NLog встроен в CpuMetricsController");
        }
        
        /// <summary>
        /// Получает метрики CPU на заданном диапазоне времени по определенному агенту
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/cpu/agent/1/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="agentId">Id зарегистрированного агента</param>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public GetByPeriodCpuMetricsApiResponse GetMetricsFromAgent(
            [FromRoute] int agentId,
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Агент: {agentId}, From:{fromTime}, To:{toTime}");
            
            var metrics = _managerRepository.GetByTimePeriodFromAgent(fromTime, toTime, agentId);

            var response = new GetByPeriodCpuMetricsApiResponse
            {
                Metrics = new List<ApiCpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<ApiCpuMetricDto>(metric));
            }

            return response;
        }

        /// <summary>
        /// Получает метрики CPU на заданном диапазоне времени по всем агентам
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/cpu/cluster/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public GetByPeriodCpuMetricsApiResponse GetMetricsFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Общие данные From:{fromTime}, To:{toTime}");

            var metrics = _managerRepository.GetByTimePeriod(fromTime, toTime);

            var response = new GetByPeriodCpuMetricsApiResponse
            {
                Metrics = new List<ApiCpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<ApiCpuMetricDto>(metric));
            }

            return response;
        }
    }
}