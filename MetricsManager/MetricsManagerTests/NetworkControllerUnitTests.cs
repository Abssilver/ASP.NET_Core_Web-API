using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager.Controllers;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsManagerTests
{
    public class NetworkControllerUnitTests
    {
        private readonly NetworkMetricsController _controller;
        private readonly Mock<INetworkMetricsManagerRepository> _repositoryMock;
        private readonly Mock<ILogger<NetworkMetricsController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public NetworkControllerUnitTests()
        {
            _repositoryMock = new Mock<INetworkMetricsManagerRepository>();
            _loggerMock = new Mock<ILogger<NetworkMetricsController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new NetworkMetricsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
        }


        [Fact]
        public void GetMetricsFromAgent_ShouldCall_GetByTimePeriodFromAgent_From_Repository()
        {
            var agentId = 1;
            var fromTime = DateTimeOffset.Now;
            var toTime = DateTimeOffset.Now;

            _repositoryMock.Setup(repository =>
                    repository.GetByTimePeriodFromAgent(
                        It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()))
                .Returns(new List<NetworkMetric>());

            _controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            _repositoryMock.Verify(repository =>
                    repository.GetByTimePeriodFromAgent(
                        It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()),
                Times.AtMostOnce());
        }


        [Fact]
        public void GetMetricsFromAllCluster_ShouldCall_GetByTimePeriod_From_Repository()
        {
            var fromTime = DateTimeOffset.Now;
            var toTime = DateTimeOffset.Now;

            _repositoryMock.Setup(repository =>
                    repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<NetworkMetric>());

            _controller.GetMetricsFromAllCluster(fromTime, toTime);

            _repositoryMock.Verify(repository =>
                    repository.GetByTimePeriod(
                        It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()),
                Times.AtMostOnce());
        }
    }
}