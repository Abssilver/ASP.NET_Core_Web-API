using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DataAccessLayer.Interfaces;
using MetricsAgent.DataAccessLayer.Models;
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
        private readonly Mock<IMapper> _mapperMock;

        public HddControllerUnitTests()
        {
            _repositoryMock = new Mock<IHddMetricsRepository>();
            _loggerMock = new Mock<ILogger<HddMetricsController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new HddMetricsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
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