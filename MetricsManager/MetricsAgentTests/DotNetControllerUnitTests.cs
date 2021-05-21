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
    public class DotNetControllerUnitTests
    {
        private readonly DotNetMetricsController _controller;
        
        private readonly Mock<IDotNetMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<DotNetMetricsController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public DotNetControllerUnitTests()
        {
            _repositoryMock = new Mock<IDotNetMetricsRepository>();
            _loggerMock = new Mock<ILogger<DotNetMetricsController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new DotNetMetricsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<DotNetMetric>());
            
            _controller.GetByTimePeriod(DateTimeOffset.Now, DateTimeOffset.Now);
            
            _repositoryMock.Verify(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
            
        }
    }
}