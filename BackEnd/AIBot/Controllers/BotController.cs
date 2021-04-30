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
using Buaa.AIBot.Services;
using Buaa.AIBot.Controllers.Models;

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
        private readonly IUserService userService;

        public BotController(IBotRunner botRunner, IUserService userService)
        {
            this.botRunner = botRunner;
            this.userService = userService;
        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("start")]
        public async Task<IActionResult> StartAsync(BotBody body)
        {
            int uid = userService.GetUidFromToken(Request);
            OutputInfo info = await botRunner.Start((uid));
            return Ok(info);
        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("message")]
        public async Task<IActionResult> MessageAsync(BotBody body)
        {
            string message = (body.Message == null)? "" : body.Message;
            int uid = userService.GetUidFromToken(Request);
            OutputInfo info = await botRunner.Run(uid, new InputInfo
            {
                Message = message
            });
            return Ok(info);
        }
    }
}
