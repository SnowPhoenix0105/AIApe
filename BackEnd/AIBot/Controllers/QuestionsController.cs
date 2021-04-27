using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buaa.AIBot.Services;

namespace Buaa.AIBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService questionService;

        public QuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [AllowAnonymous]
        [HttpGet("question")]
        public async Task<IActionResult> QuestionAsync()
        {

        }

        [AllowAnonymous]
        [HttpGet("answer")]
        public async Task<IActionResult> AnswerAsync()
        {

        }

        [AllowAnonymous]
        [HttpGet("tag")]
        public async Task<IActionResult> TagAsync()
        {

        }

        [AllowAnonymous]
        [HttpPost("questionlist")]
        public async Task<IActionResult> QuestionlistAsync()
        {

        }

        [AllowAnonymous]
        [HttpGet("taglist")]
        public async Task<IActionResult> TaglistAsync()
        {

        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("add_question")]
        public async Task<IActionResult> AddQuestionAsync()
        {

        }

        [Authorize(Policy = "Admin")]
        [HttpPost("add_tag")]
        public async Task<IActionResult> AddTagAsync()
        {

        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPut("modify_question")]
        public async Task<IActionResult> ModifyQuestion()
        {

        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPut("modify_answer")]
        public async Task<IActionResult> ModifyAnswer()
        {

        }

        [Authorize(Policy = "Admin")]
        [HttpPut("modify_tag")]
        public async Task<IActionResult> ModifyTag()
        {

        }

        [Authorize(Policy = "UserAdmin")]
        [HttpDelete("delete_question")]
        public async Task<IActionResult> DeleteQuestion()
        {

        }

        [Authorize(Policy = "UserAdmin")]
        [HttpDelete("delete_answer")]
        public async Task<IActionResult> DeleteAnswer()
        {

        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("delete_tag")]
        public async Task<IActionResult> DeleteTag()
        {
            
        }
    }
}
