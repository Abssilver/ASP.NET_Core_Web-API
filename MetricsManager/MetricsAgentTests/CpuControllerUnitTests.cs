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
    public class CpuControllerUnitTests
    {
        private readonly CpuMetricsController _controller;
        private readonly Mock<ICpuMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<CpuMetricsController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public CpuControllerUnitTests()
        {
            _repositoryMock = new Mock<ICpuMetricsRepository>();
            _loggerMock = new Mock<ILogger<CpuMetricsController>>();
            _mapperMock = new Mock<IMapper>();
                
            _controller = new CpuMetricsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
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