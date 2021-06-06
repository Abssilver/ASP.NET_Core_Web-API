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
    public class RamControllerUnitTests
    {
        private readonly RamMetricsController _controller;

        private readonly Mock<IRamMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<RamMetricsController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public RamControllerUnitTests()
        {
            _repositoryMock = new Mock<IRamMetricsRepository>();
            _loggerMock = new Mock<ILogger<RamMetricsController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new RamMetricsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
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