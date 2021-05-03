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
    public class RamControllerUnitTests
    {
        private readonly RamMetricsController _controller;

        private readonly Mock<IRamMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<RamMetricsController>> _loggerMock;

        public RamControllerUnitTests()
        {
            _repositoryMock = new Mock<IRamMetricsRepository>();
            _loggerMock = new Mock<ILogger<RamMetricsController>>();

            _controller = new RamMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<RamMetric>())).Verifiable();

            _controller.Create(new MetricsAgent.Requests.RamMetricCreateRequest
            {
                Time = DateTimeOffset.Now,
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<RamMetric>()), Times.AtMostOnce());
        }
        
        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<RamMetric>());
            
            _controller.GetByTimePeriod(DateTimeOffset.Now, DateTimeOffset.Now);
            
            _repositoryMock.Verify(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
            
        }
    }
}