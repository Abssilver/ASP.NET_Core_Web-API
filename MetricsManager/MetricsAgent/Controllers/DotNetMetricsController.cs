using System;
using System.Collections.Generic;
using MetricsAgent.DataAccessLayer;
using MetricsAgent.Metrics;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
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

        public DotNetMetricsController(IDotNetMetricsRepository repository, ILogger<DotNetMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogInformation(1, "NLog встроен в DotNetMetricsController");
        }

        //TODO: по замечанию create быть не должно
        //http://localhost:51684/api/metrics/dotnet/create
        //{ "Time": "2021-05-02", "Value": 100 }
        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _logger.LogInformation($"Создается запись с данными Time:{request.Time}; Value:{request.Value}");

            _repository.Create(new DotNetMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }

        //http://localhost:51684/api/metrics/dotnet/errors-count/from/2021-04-10/to/2021-05-03
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
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
                response.Metrics.Add(new DotNetMetricDto
                {
                    Time = metric.Time, 
                    Value = metric.Value, 
                    Id = metric.Id
                });
            }

            return Ok(response);
        }
    }
}