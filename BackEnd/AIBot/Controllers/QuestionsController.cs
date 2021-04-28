using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buaa.AIBot;
using Buaa.AIBot.Services;
using Buaa.AIBot.Services.Models;
using Buaa.AIBot.Services.Exceptions;
using Buaa.AIBot.Controllers.Models;

namespace Buaa.AIBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService questionService;

        private readonly IUserService userService;

        public QuestionsController(IQuestionService questionService, IUserService userService)
        {
            this.questionService = questionService;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("question")]
        public async Task<IActionResult> QuestionAsync()
        {
            int qid = int.Parse(Request.Query["qid"]);
            try
            {
                QuestionInformation question = await questionService.GetQuestionAsync(qid);
                return Ok(new
                {
                    Status = "success",
                    Message = $"get question with qid={qid} successfully",
                    Question = question
                });
            } catch (QuestionNotExistException) {
                return NotFound(new 
                {
                    Status = "questionNotExist",
                    Message = $"question with qid={qid} dose not exist",
                    QuestionAsync= new QuestionInformation()
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("answer")]
        public async Task<IActionResult> AnswerAsync()
        {
            int aid = int.Parse(Request.Query["aid"]);
            try
            {
                AnswerInformation answer = await questionService.GetAnswerAsync(aid);
                return Ok(new
                {
                    Status = "success",
                    Message = $"get answer with aid={aid} successfully",
                    Answer = answer
                });
            } catch (AnswerNotExistException) {
                return NotFound(new
                {
                    Status = "answerNotExist",
                    Message = $"answer with aid={aid} dose not exist",
                    Answer = new AnswerInformation()
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("tag")]
        public async Task<IActionResult> TagAsync()
        {
            int tid = int.Parse(Request.Query["tid"]);
            try
            {
                TagInformation tag = await questionService.GetTagAsync(tid);
                return Ok(new
                {
                    Status = "success",
                    Message = $"get tag with tid={tid} successfully",
                    Tag = tag
                });
            } catch (TagNotExistException) {
                return NotFound(new
                {
                    Status = "tagNotExist",
                    Message = $"tag with tid={tid} dose not exist",
                    Tag = new TagInformation()
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("questionlist")]
        public async Task<IActionResult> QuestionlistAsync(QuestionBody body)
        {
            IEnumerable<int> tags = (body.Tags == null)? new int[0] : body.Tags;
            int? pt = body.Pt;
            int number = body.Number.GetValueOrDefault(Constants.QuestionListMaxNumber);
            return Ok(await questionService.GetQuestionListAsync(tags, pt, number));
        }

        [AllowAnonymous]
        [HttpGet("taglist")]
        public async Task<IActionResult> TaglistAsync()
        {
            return Ok(await questionService.GetTagListAsync());
        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("add_question")]
        public async Task<IActionResult> AddQuestionAsync(QuestionBody body)
        {
            int creater = userService.GetUidFromToken(Request);
            string title = (body.Title == null)? "" : body.Title;
            string remarks = (body.Remarks == null)? "" : body.Remarks;
            IEnumerable<int> tags = (body.Tags == null)? new int[0] : body.Tags;
            try
            {
                int qid = await questionService.AddQuestionAsync(creater, title, remarks, tags);
                return Ok(new
                {
                    Status = "success",
                    Message = "new question add successfully"
                });
            } catch (QuestionTitleTooLongException) {
                return Ok(new
                {
                    Status = "questionTooLong",
                    Message = "describtion of question is too long"
                });
            } catch (UserNotExistException) {
                return Ok(new
                {
                    Status = "userNotExist",
                    Message = "adding question fail for user problem"
                });
            } catch (TagNotExistException) {
                return Ok(new
                {
                    Status = "ragNotExist",
                    Message = "can not resolve tags"
                });
            }
        }

        [Authorize(Policy = "UserAdmin")]
        [HttpPost("add_answer")]
        public async Task<IActionResult> AddAnswerAsync()
        {
            int creater = userService.GetUidFromToken(Request);
            int qid =    
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
