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
    public class NetworkControllerUnitTests
    {
        private readonly NetworkMetricsController _controller;

        private readonly Mock<INetworkMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<NetworkMetricsController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public NetworkControllerUnitTests()
        {
            _repositoryMock = new Mock<INetworkMetricsRepository>();
            _loggerMock = new Mock<ILogger<NetworkMetricsController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new NetworkMetricsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<NetworkMetric>());
            
            _controller.GetByTimePeriod(DateTimeOffset.Now, DateTimeOffset.Now);
            
            _repositoryMock.Verify(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
            
        }
    }
}