using System;
using MetricsAgent.Controllers;
using MetricsAgent.DataAccessLayer;
using MetricsAgent.Metrics;
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

        public DotNetControllerUnitTests()
        {
            _repositoryMock = new Mock<IDotNetMetricsRepository>();
            _loggerMock = new Mock<ILogger<DotNetMetricsController>>();

            _controller = new DotNetMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<DotNetMetric>())).Verifiable();

            _controller.Create(new MetricsAgent.Requests.DotNetMetricCreateRequest
            {
                Time = DateTimeOffset.Now,
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<DotNetMetric>()), Times.AtMostOnce());
        }
        
        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Verifiable();
            
            _controller.GetByTimePeriod(DateTimeOffset.Now, DateTimeOffset.Now);
            
            _repositoryMock.Verify(repository =>
                repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
            
        }
    }
}