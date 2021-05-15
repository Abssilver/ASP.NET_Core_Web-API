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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsRepository _repository;

        public CpuMetricsController(ICpuMetricsRepository repository, ILogger<CpuMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogInformation(1, "NLog встроен в CpuMetricsController");
        }

        //TODO: по замечанию create быть не должно
        //http://localhost:51684/api/metrics/cpu/create
        //{ "Time": "2021-05-02", "Value": 100 }
        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            _logger.LogInformation($"Создается запись с данными Time:{request.Time}; Value:{request.Value}");

            _repository.Create(new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }

        //http://localhost:51684/api/metrics/cpu/from/2021-04-10/to/2021-05-03
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Запрос записи From:{fromTime}; To:{toTime}");

            var metrics = _repository.GetByTimePeriod(fromTime, toTime);

            var response = new GetByPeriodCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto
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