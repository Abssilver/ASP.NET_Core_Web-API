using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MetricsAgentTests
{
    public class RamControllerUnitTests
    {
        private readonly RamMetricsController _controller;

        public RamControllerUnitTests()
        {
            _controller = new RamMetricsController();
        }
        

        [Fact]
        public void GetFreeRamSizeMetrics_ReturnsOk()
        {
            var result = _controller.GetFreeRamSizeMetrics();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }

}