using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using microservice.Properties;

namespace microservice.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ApiVersion("1.0")]
        public IActionResult GetV1()
        {
            _logger.LogInformation("Api V1");
            _logger.LogError("Log error Api V1");
            return Ok(Resource.MSG_ONE);
        }
        [HttpGet]
        [ApiVersion("2.0")]
        public IActionResult GetV2()
        {
            return Ok("v2");
        }
    }
}
