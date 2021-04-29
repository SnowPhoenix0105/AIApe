// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EmptyBot .NET Template version v4.12.2

using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotRunner botRunner;

        public BotController(IBotRunner botRunner)
        {
            this.botRunner = botRunner;
        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("start")]
        public async Task<IActionResult> Start()
        {
            
        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("policy")]
        public async Task<IActionResult> Message()
        {

        }
    }
}
