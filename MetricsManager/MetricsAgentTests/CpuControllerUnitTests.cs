using System;
using MetricsAgent.Controllers;
using MetricsManager.Enums;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuControllerUnitTests
    {
        private readonly CpuMetricsController _controller;

        public CpuControllerUnitTests()
        {
            _controller = new CpuMetricsController();
        }

        [Fact]
        public void GetMetrics_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = _controller.GetMetrics(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        
        
        [Theory]
        [InlineData(0, 100, Percentile.Median)]
        [InlineData(0, 100, Percentile.P75)]
        [InlineData(0, 100, Percentile.P90)]
        [InlineData(0, 100, Percentile.P95)]
        [InlineData(0, 100, Percentile.P99)]
        public void GetMetricsByPercentile_ReturnsOk(
            int start,
            int end,
            Percentile percentile)
        {
            var fromTime = TimeSpan.FromSeconds(start);
            var toTime = TimeSpan.FromSeconds(end);

            var result = _controller.GetMetricsByPercentile(fromTime, toTime, percentile);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }

}