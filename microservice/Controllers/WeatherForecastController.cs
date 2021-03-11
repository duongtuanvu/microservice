using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using microservice.Commands;
using microservice.Models;
using microservice.Properties;
using microservice.Validators;

namespace microservice.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IMediator mediatR, ILogger<WeatherForecastController> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetV1()
        {
            return Ok(Resource.MSG_ONE);
        }
        [HttpPost]
        public async Task<IActionResult> PostV1([FromBody] UserCreateCommand user)
        {
            await _mediatR.Send(user);
            return Ok(user);
        }
        [HttpGet]
        [ApiVersion("2.0")]
        public IActionResult GetV2()
        {
            return Ok("v2");
        }
    }
}
