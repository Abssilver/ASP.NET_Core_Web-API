using System;
using Microsoft.AspNetCore.Mvc;
using MetricsManager.Model;

namespace MetricsManager.Controllers
{
    [Route("api/crud")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly TemperatureModel _temperatureModel;

        public CrudController(TemperatureModel temperatureModel)
        {
            _temperatureModel = temperatureModel;
        }

        //http://localhost:51684/api/crud/create?date=2021-04-17&temperature=9
        [HttpPost("create")]
        public IActionResult Create([FromQuery] DateTime date, [FromQuery] int temperature)
        {
            _temperatureModel.AddValue(date, temperature);
            return Ok();
        }
        //http://localhost:51684/api/crud/read_all
        [HttpGet("read_all")]
        public IActionResult ReadAll()
        {
            return Ok(_temperatureModel.GetTemperatureValues());
        }
        //http://localhost:51684/api/crud/read?from=2021-04-19&to=2021-04-21
        [HttpGet("read")]
        public IActionResult Read([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return Ok(_temperatureModel.GetTemperatureValues(from, to));
        }
        //http://localhost:51684/api/crud/update?date=2021-04-21&temperature=6
        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime date, [FromQuery] int temperature)
        {
            _temperatureModel.UpdateValue(date, temperature);
            return Ok();
        }
        //http://localhost:51684/api/crud/delete?date=2021-04-22
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime date)
        {
            _temperatureModel.DeleteValue(date);
            return Ok();
        }
        //http://localhost:51684/api/crud/delete_range?from=2021-04-17&to=2021-04-21
        [HttpDelete("delete_range")]
        public IActionResult Delete([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            _temperatureModel.DeleteRange(from, to);
            return Ok();
        }
    }
}
