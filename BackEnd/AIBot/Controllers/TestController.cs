using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Mime;

namespace Buaa.AIBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("coffee")]
        public StatusCodeResult Coffee()
        {
            return StatusCode(StatusCodes.Status418ImATeapot);
        }

        [HttpPost("echojson")]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult EchoJson([FromBody] JsonDocument body)
        {
            return Ok(body);
        }
    }
}
