using System;
using MetricsManager.Controllers;
using MetricsManager.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuControllerUnitTests
    {
        private readonly CpuMetricsController _controller;

        public CpuControllerUnitTests()
        {
            var loggerMock = new Mock<ILogger<CpuMetricsController>>();

            _controller = new CpuMetricsController(loggerMock.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        
        
        [Theory]
        [InlineData(1, 0, 100, Percentile.Median)]
        [InlineData(1, 0, 100, Percentile.P75)]
        [InlineData(1, 0, 100, Percentile.P90)]
        [InlineData(1, 0, 100, Percentile.P95)]
        [InlineData(1, 0, 100, Percentile.P99)]
        public void GetMetricsByPercentileFromAgent_ReturnsOk(
            int agentId,
            int start,
            int end,
            Percentile percentile)
        {
            var fromTime = TimeSpan.FromSeconds(start);
            var toTime = TimeSpan.FromSeconds(end);
            
            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        
        
        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        
        
        [Theory]
        [InlineData(0, 100, Percentile.Median)]
        [InlineData(0, 100, Percentile.P75)]
        [InlineData(0, 100, Percentile.P90)]
        [InlineData(0, 100, Percentile.P95)]
        [InlineData(0, 100, Percentile.P99)]
        public void GetMetricsByPercentileFromAllCluster_ReturnsOk(
            int start,
            int end,
            Percentile percentile)
        {
            var fromTime = TimeSpan.FromSeconds(start);
            var toTime = TimeSpan.FromSeconds(end);

            var result = _controller.GetMetricsByPercentileFromAllCluster(fromTime, toTime, percentile);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }

}