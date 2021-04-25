using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buaa.AIBot.Services;
using Buaa.AIBot.Controllers.Models;

namespace Buaa.AIBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summmary>
        /// User sign up.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp(UserInfo body)
        {
            return Ok(new StatusMessageResponse {Status = "success", Message = "signup success"});
        }

        /// <summary>
        /// User login.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserInfo body)
        {
            return Ok(new {Status = "success", Message = "login success", Token = "1", Auth = 1, Timeout = 600});
        }

        /// <summary>
        /// Get public infomation.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("public_info")]
        public IActionResult GetPublicInfo()
        {
            int uid = int.Parse(HttpContext.Request.Query["uid"]);
            return Ok(new {Status = "success", Name = ""});
        }

        /// <summary>
        /// Get internal infomation.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("internal_info")]
        public IActionResult GetInternalInfo()
        {
            int uid = int.Parse(HttpContext.Request.Query["uid"]);
            return Ok(new {Status = "", Uid = 0, Name = "", Email = ""});
        }

        /// <summary>
        /// Get full infomation.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("full_info")]
        public IActionResult GetFullInfo()
        {
            int uid = int.Parse(HttpContext.Request.Query["uid"]);
            return Ok(new {Status = "", Uid = 0, Name = "", Email = "", Auth = 0});
        }

        /// <summary>
        /// Modify full information.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpPut("modify")]
        public IActionResult Modify(UserInfo body)
        {
            return Ok(new List<StatusMessageResponse>());
        }

        /// <summary>
        /// List questions of a specific user.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("questions")]
        public IActionResult Questions()
        {
            int uid = int.Parse(HttpContext.Request.Query["uid"]);
            return Ok(new {Status = "success", Message = "123", Questions = new List<int>{120, 100, 92, 21}});
        }

        /// <summary>
        /// List questions of a specific user.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("answers")]
        public IActionResult Answers()
        {
            int uid = int.Parse(HttpContext.Request.Query["uid"]);
            return Ok(new {Status = "success", Message = "123", Answers = new List<int>{1, 2, 3}});
        }

        /// <summary>
        /// Fresh token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fresh")]
        public IActionResult Fresh(UserInfo body)
        {
            return Ok(new {Status = "", Message = "", Token = "", Timeout = 0});
        }
    }
}
