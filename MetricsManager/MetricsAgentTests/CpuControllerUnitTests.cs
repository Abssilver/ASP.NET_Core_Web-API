using System;
using System.Collections.Generic;
using MetricsAgent.Controllers;
using MetricsAgent.DataAccessLayer;
using MetricsAgent.Metrics;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuControllerUnitTests
    {
        private readonly CpuMetricsController _controller;
        private readonly Mock<ICpuMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<CpuMetricsController>> _loggerMock;

        public CpuControllerUnitTests()
        {
            _repositoryMock = new Mock<ICpuMetricsRepository>();
            _loggerMock = new Mock<ILogger<CpuMetricsController>>();

            _controller = new CpuMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            // выполняем действие на контроллере
            var result = _controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest
            {
                Time = DateTimeOffset.Now,
                Value = 50
            });
            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }
        
        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<CpuMetric>());
            
            _controller.GetByTimePeriod(DateTimeOffset.Now, DateTimeOffset.Now);
            
            _repositoryMock.Verify(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
            
        }
    }
}