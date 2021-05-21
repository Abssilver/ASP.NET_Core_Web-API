using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.Responses;
using MetricsAgent.Responses.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _repository;
        private readonly IMapper _mapper;

        public DotNetMetricsController(
            IDotNetMetricsRepository repository,
            ILogger<DotNetMetricsController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogInformation(1, "NLog встроен в DotNetMetricsController");
        }
        
        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса (Допускается также ввод временной метки в формате 2021-05-14):
        ///
        ///     GET url:port/api/metrics/dotnet/from/2021-05-14T00:00:00/to/2022-06-20T00:00:00
        /// 
        /// </remarks>
        /// <param name="fromTime">Начальная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <param name="toTime">Конечная метка времени с 01.01.1970 в формате DateTimeOffset</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Все хорошо</response>
        /// <response code="400">Передали неправильные параметры</response>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public GetByPeriodDotNetMetricsResponse GetByTimePeriod(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Запрос записи From:{fromTime}; To:{toTime}");

            var metrics = _repository.GetByTimePeriod(fromTime, toTime);

            var response = new GetByPeriodDotNetMetricsResponse
            {
                Metrics = new List<DotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
            }

            return response;
        }
    }
}