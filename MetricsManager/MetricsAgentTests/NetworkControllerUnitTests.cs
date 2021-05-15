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
    public class NetworkControllerUnitTests
    {
        private readonly NetworkMetricsController _controller;

        private readonly Mock<INetworkMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<NetworkMetricsController>> _loggerMock;

        public NetworkControllerUnitTests()
        {
            _repositoryMock = new Mock<INetworkMetricsRepository>();
            _loggerMock = new Mock<ILogger<NetworkMetricsController>>();

            _controller = new NetworkMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<NetworkMetric>())).Verifiable();

            _controller.Create(new MetricsAgent.Requests.NetworkMetricCreateRequest
            {
                Time = DateTimeOffset.Now,
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<NetworkMetric>()), Times.AtMostOnce());
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