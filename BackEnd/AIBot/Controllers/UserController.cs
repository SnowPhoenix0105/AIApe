using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buaa.AIBot.Services;
using Buaa.AIBot.Controllers.Models;
using Buaa.AIBot.Repository.Models;
using Buaa.AIBot.Repository;

using BNBCrypt = BCrypt.Net.BCrypt;

namespace Buaa.AIBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        
        private readonly IUserRepository userRepository;

        private readonly TokenManagement tokenManagement;

        private static readonly Dictionary<AuthLevel, int> intAuth = new Dictionary<AuthLevel, int>
        {
            {AuthLevel.None, 0},
            {AuthLevel.User, 1},
            {AuthLevel.Admin, 2}
        };

        private static readonly Dictionary<string, int> intAuthFromString = new Dictionary<string, int>
        {
            {"Traveler", 0},
            {"User", 1},
            {"Administrator", 2}
        };


        private static readonly Dictionary<int, AuthLevel> AuthLevelFromInt = new Dictionary<int, AuthLevel>
        {
            {0, AuthLevel.None},
            {1, AuthLevel.User},
            {2, AuthLevel.Admin}
        };

        public UserController(IUserService userService, IUserRepository userRepository,
                              IOptions<TokenManagement> tokenManagement)
        {
            this.userService = userService;
            this.userRepository = userRepository;
            this.tokenManagement = tokenManagement.Value;
        }

        /// <summmary>
        /// User sign up.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(UserBody body)
        {
            StatusMessageResponse response;
            if (!userService.CheckSignUpBody(body, out response))
            {
                return Ok(response);
            }
            try{
                UserInfo newUser = new UserInfo
                {
                    Email = body.Email,
                    Name = body.Name,
                    Bcrypt = BNBCrypt.HashPassword(body.Password),
                    Auth = AuthLevel.User
                };
                await userRepository.InsertUserAsync(newUser);
                return Ok(new StatusMessageResponse 
                {
                    Status = "success",
                    Message = "signup success"
                });
            } catch (EmailHasExistException) {
                return Ok(new StatusMessageResponse
                {
                    Status = "emailExisted",
                    Message = "email has been used"
                });
            } catch (NameHasExistException) {
                return Ok(new StatusMessageResponse
                {
                    Status = "nameExisted",
                    Message = "name has been used"
                });
            }
        }

        /// <summary>
        /// User login.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserBody body)
        {
            string[] tokenAndAuth = await userService.AuthorizeAccountAsync(body);
            if (tokenAndAuth[0].Equals(string.Empty))
            {
                return Ok(new 
                {
                    Status = "fail",
                    Message = "wrong email or password",
                    Token = "",
                    Auth = intAuth[AuthLevel.None],
                    Timeout = 0
                });
            }  
            return Ok(new 
            {
                Status = "success",
                Message = "login success",
                Token = tokenAndAuth[0],
                Auth = intAuthFromString[tokenAndAuth[1]],
                Timeout = tokenManagement.AccessExpiration * 60
            });
        }

        /// <summary>
        /// Get public infomation.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("public_info")]
        public async Task<IActionResult> GetPublicInfoAsync()
        {
            int uid = int.Parse(HttpContext.Request.Query["uid"]);
            UserInfo userInfo = await userRepository.SelectUserByIdAsync(uid);
            if (userInfo == null)
            {
                return NotFound(new 
                {
                    Status = "userNotExist",
                    Name = "UNKNOWN"
                });
            }
            return Ok(new 
            {
                Status = "success",
                Name = userInfo.Name
            });
        }

        /// <summary>
        /// Get internal infomation.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("internal_info")]
        public async Task<IActionResult> GetInternalInfoAsync()
        {
            int uid;
            uid = userService.GetUidFromParameters(Request);
            uid = (uid == -1)? userService.GetUidFromToken(Request) : uid;
            UserInfo userInfo = await userRepository.SelectUserByIdAsync(uid);
            if (userInfo == null)
            {
                return Ok(new 
                {
                    Status = "success",
                    Uid = userInfo.UserId,
                    Name = userInfo.Name,
                    Email = userInfo.Email
                });
            } else {
                return NotFound(new 
                {
                    Status = "userNotExist",
                    Uid = -1,
                    name = "UNKNOWN",
                    Email = "N@N"
                });
            }
        }

        /// <summary>
        /// Get full infomation.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("full_info")]
        public async Task<IActionResult> GetFullInfoAsync()
        {
            int presentUid = userService.GetUidFromToken(Request);
            int uid;
            uid = userService.GetUidFromParameters(Request);
            uid = (uid == -1)? presentUid : uid;
            if(uid != presentUid)
            {
                AuthLevel presentAuth = userService.GetAuthLevelFromToken(Request);
                if (presentAuth != AuthLevel.Admin)
                {
                    return Unauthorized(new 
                    {
                        Auth = intAuth[presentAuth]
                    });
                }
            }
            UserInfo userInfo = await userRepository.SelectUserByIdAsync(uid);
            if (userInfo == null)
            {
                return NotFound(new 
                {
                    Uid = uid
                });
            } else {
                return Ok(new 
                {
                    Status = "success",
                    Uid = userInfo.UserId,
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Auth = intAuth[userInfo.Auth]
                });
            }
        }

        /// <summary>
        /// Modify full information.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpPut("modify")]
        public async Task<IActionResult> ModifyAsync(UserBody body)
        {
            int presentUid = userService.GetUidFromToken(Request);
            AuthLevel presentAuth = AuthLevel.None;
            int uid = body.Uid;
            if (uid <= 0)
            {
                uid = presentUid;
            }
            if (uid != presentUid)
            {
                presentAuth = userService.GetAuthLevelFromToken(Request);
                if (presentAuth != AuthLevel.Admin)
                {
                    return Unauthorized(new 
                    {
                        Message = "lack of authority"
                    });
                }
            }
            UserInfo userInfo = await userRepository.SelectUserByIdAsync(uid);
            if (userInfo == null)
            {
                return Conflict(new
                {
                    Status = "userNotExist",
                    Message = "user dose not exist"
                });
            }
            if (body.Name != null)
            {
                if (!userService.CheckName(body.Name))
                {
                    return Conflict(new 
                    {
                        Status = "nameInvalid",
                        Message = "either contains illegal characters or length of name is too long or too short"
                    });
                }
                userInfo.Name = body.Name;
            }
            if (body.Password != null)
            {
                if (!userService.CheckPassword(body.Password))
                {
                    return Conflict(new
                    {
                        Status = "passwordInvalid",
                        Message = "either contains illegal characters or length of password is too long or too short"
                    });
                }
                if (BNBCrypt.Verify(body.Password, userInfo.Bcrypt))
                {
                    return Conflict(new
                    {
                        Status = "passwordSame",
                        Message = "new password can not be the same as original one"
                    });
                }
                userInfo.Bcrypt = BNBCrypt.HashPassword(body.Password);
            }
            if (body.Auth != 0)
            {
                if (presentAuth == AuthLevel.None)
                {
                    presentAuth = userService.GetAuthLevelFromToken(Request);
                }
                if (presentAuth != AuthLevel.Admin)
                {
                    return Unauthorized(new
                    {
                        Message = "lack of authority"
                    });
                }
                if (uid == presentUid)
                {
                    return Conflict(new
                    {
                        Status = "tryChangeSelfAuth",
                        Message = "try to change authority of current user"
                    });
                }
                if (body.Auth < 1)
                {
                    body.Auth = 1;
                }
                userInfo.Auth = AuthLevelFromInt[body.Auth];
            }
            try
            {
                await userRepository.UpdateUserAsync(userInfo);
            } catch (NameHasExistException) {
                return Conflict(new
                {
                    Status = "NameExisted",
                    Message = "name has been used by other user"
                });
            }

            return Ok(new
            {
                Status = "success",
                Message = "modify successfully"
            });
        }

        /// <summary>
        /// List questions of a specific user.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("questions")]
        public async Task<IActionResult> QuestionsAsync()
        {
            int uid;
            uid = userService.GetUidFromParameters(Request);
            uid = (uid == -1)? userService.GetUidFromToken(Request) : uid;
            IEnumerable<int> qids = await userRepository.SelectQuestionsIdByIdOrderByModifyTimeAsync(uid);
            if (qids == null)
            {
                return NotFound(new
                {
                    Status = "userNotExist",
                    Message = "user dose not exist"
                });
            } else {
                return Ok(new
                {
                    Status = "success",
                    Message = $"get questions for user{uid} successfully",
                    Questions = qids
                });
            }
        }

        /// <summary>
        /// List questions of a specific user.
        /// </summary>
        [Authorize(Policy = "UserAdmin")]
        [HttpGet("answers")]
        public async Task<IActionResult> AnswersAsync()
        {
            int uid;
            uid = userService.GetUidFromParameters(Request);
            uid = (uid == -1)? userService.GetUidFromToken(Request) : uid;
            IEnumerable<int> aids = await userRepository.SelectAnswersIdByIdByModifyTimeAsync(uid);
            if (aids == null)
            {
                return NotFound(new
                {
                    Status = "userNotExist",
                    Message = "user dose not exist"
                });
            } else {
                return Ok(new
                {
                    Status = "success",
                    Message = $"get answers for user{uid} successfully",
                    Answers = aids
                });
            }
        }

        /// <summary>
        /// Fresh token, leave alone if the user has changed his password.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fresh")]
        public async Task<IActionResult> FreshAsync(UserBody body)
        {
            DateTime expiration;
            try
            {
                expiration = userService.GetExpirationFromToken(Request);
                if (expiration.CompareTo(DateTime.Now) < 0)
                {
                    return Unauthorized(new
                    {
                        Message = "token has been expried"
                    });
                }
            } catch (ArgumentException) {
                return Unauthorized(new
                {
                    Message = "invalid token"
                });
            }
            try
            {
                string newToken = await userService.FleshTokenAsync(Request);
                return Ok(new
                {
                    Status = "success",
                    Message = "token fresh",
                    Token = newToken,
                    Timeout = tokenManagement.AccessExpiration * 60
                });
            } catch (UserNotExistException) {
                return Unauthorized(new
                {
                    Message = "user has been removed"
                });
            }
        }
    }
}
