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
    public class HddControllerUnitTests
    {
        private readonly HddMetricsController _controller;

        private readonly Mock<IHddMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<HddMetricsController>> _loggerMock;

        public HddControllerUnitTests()
        {
            _repositoryMock = new Mock<IHddMetricsRepository>();
            _loggerMock = new Mock<ILogger<HddMetricsController>>();

            _controller = new HddMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<HddMetric>())).Verifiable();

            _controller.Create(new MetricsAgent.Requests.HddMetricCreateRequest
            {
                Time = DateTimeOffset.Now,
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }
        
        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<HddMetric>());
            
            _controller.GetByTimePeriod(DateTimeOffset.Now, DateTimeOffset.Now);
            
            _repositoryMock.Verify(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
            
        }
    }
}