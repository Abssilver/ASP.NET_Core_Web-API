using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MetricsAgentTests
{
    public class HddControllerUnitTests
    {
        private readonly HddMetricsController _controller;

        public HddControllerUnitTests()
        {
            _controller = new HddMetricsController();
        }
        
        
        [Fact]
        public void GetRemainingFreeDiskSpaceMetrics_ReturnsOk()
        {
            var result = _controller.GetRemainingFreeDiskSpaceMetrics();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }

}