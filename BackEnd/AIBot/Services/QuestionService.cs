using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Services.Models;
using Buaa.AIBot;
using Buaa.AIBot.Services.Exceptions;
using Buaa.AIBot.Repository;
using Buaa.AIBot.Repository.Models;
using Buaa.AIBot.Repository.Exceptions;

namespace Buaa.AIBot.Services
{
    /// <summary>
    /// Implemention of <see cref="IQuestionService"/>
    /// </summary>
    /// <remarks><seealso cref="IQuestionService"/></remarks>
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly ITagRepostory tagRepostory;

        public QuestionService(IQuestionRepository question, IAnswerRepository answerRepository, ITagRepostory tagRepostory)
        {
            this.questionRepository = question;
            this.answerRepository = answerRepository;
            this.tagRepostory = tagRepostory;
        }

        public async Task<QuestionInformation> GetQuestionAsync(int qid)
        {
            var question = await questionRepository.SelectQuestionByIdAsync(qid);
            if (question == null)
            {
                throw new Exceptions.QuestionNotExistException(qid);
            }
            var answers = await questionRepository.SelectAnswersForQuestionByIdAsync(qid);
            if (answers == null)
            {
                throw new Exceptions.QuestionNotExistException(qid);
            }
            var tags = await questionRepository.SelectTagsForQuestionByIdAsync(qid);
            if (tags == null)
            {
                throw new Exceptions.QuestionNotExistException(qid);
            }
            return new QuestionInformation()
            {
                Title = question.Title,
                Remarks = question.Remarks,
                Creater = question.CreaterId,
                Best = question.BestAnswerId,
                CreatTime = question.CreateTime,
                ModifyTime = question.ModifyTime,
                Tags = new Dictionary<string, int>(tags),
                Answers = answers
            };
        }

        public async Task<AnswerInformation> GetAnswerAsync(int aid)
        {
            var answer = await answerRepository.SelectAnswerByIdAsync(aid);
            if (answer == null)
            {
                throw new Exceptions.AnswerNotExistException(aid);
            }
            return new AnswerInformation()
            {
                Content = answer.Content,
                Creater = answer.CreaterId,
                CreateTime = answer.CreateTime,
                ModifyTime = answer.ModifyTime
            };
        }

        public async Task<TagInformation> GetTagAsync(int tid)
        {
            var tag = await tagRepostory.SelectTagByIdAsync(tid);
            if (tag == null)
            {
                throw new Exceptions.TagNotExistException(tid);
            }
            return new TagInformation()
            {
                Name = tag.Name,
                Desc = tag.Desc
            };
        }

        public async Task<IEnumerable<int>> GetQuestionListAsync(IEnumerable<int> tags, int? pt, int number)
        {
            if (number < 0)
            {
                return new int[0];
            }
            var questions = await questionRepository.SelectQuestionsByTagsAsync(tags);
            int upper = pt ?? int.MaxValue;
            var res = from qid in questions
                      where qid < upper
                      orderby qid descending
                      select qid;
            var ret = new List<int>();
            if (number > Constants.QuestionListMaxNumber)
            {
                number = Constants.QuestionListMaxNumber;
            }
            foreach (var qid in res)
            {
                if (number <= 0)
                {
                    break;
                }
                ret.Add(qid);
                number--;
            }
            return ret;
        }

        public Task<Dictionary<string, int>> GetTagListAsync()
        {
            return tagRepostory.SelectAllTagsAsync();
        }

        public async Task<int> AddQuestionAsync(int creater, string title, string remarks, IEnumerable<int> tags)
        {
            if (title.Length > Constants.QuestionTitleMaxLength)
            {
                throw new Exceptions.QuestionTitleTooLongException(title.Length, Constants.QuestionTitleMaxLength);
            }
            try
            {
                int qid = await questionRepository.InsertQuestionAsync(new QuestionWithListTag()
                {
                    CreaterId = creater,
                    Title = title,
                    Remarks = remarks,
                    Tags = tags
                });
                return qid;
            }
            catch (Repository.Exceptions.UserNotExistException e)
            {
                throw new Exceptions.UserNotExistException(creater, e);
            }
            catch (Repository.Exceptions.TagNotExistException e)
            {
                throw new Exceptions.TagNotExistException(e.TagId, e);
            }
        }

        public async Task<int> AddAnswerAsync(int creater, int qid, string content)
        {
            try
            {
                int aid = await answerRepository.InsertAnswerAsync(new AnswerInfo()
                {
                    CreaterId = creater,
                    QuestionId = qid,
                    Content = content
                });
                return aid;
            }
            catch (Repository.Exceptions.QuestionNotExistException e)
            {
                throw new Exceptions.QuestionNotExistException(qid, e);
            }
            catch (Repository.Exceptions.UserHasAnswerTheQuestionException e)
            {
                throw new Exceptions.UserHasAnswerTheQuestionException(uid: creater, qid: qid, e);
            }
        }

        public async Task<int> AddTagAsync(string name, string desc)
        {
            try
            {
                int tid = await tagRepostory.InsertTagAsync(new TagInfo()
                {
                    Name = name,
                    Desc = desc
                });
                return tid;
            }
            catch (TagNameHasExistException e)
            {
                throw new TagNameExistException(name, e);
            }
        }

        public async Task ModifyQuestionAsync(int qid, QuestionModifyItems modifyItems)
        {
            try
            {
                await questionRepository.UpdateQuestionAsync(new QuestionWithListTag()
                {
                    QuestionId = qid,
                    Title = modifyItems.Title,
                    Remarks = modifyItems.Remarks,
                    BestAnswerId = modifyItems.BestAnswer,
                    Tags = modifyItems.Tags
                });
            }
            catch (Repository.Exceptions.TagNotExistException e)
            {
                throw new Exceptions.TagNotExistException(e.TagId, e);
            }
        }

        public async Task ModifyAnswerAsync(int aid, string content)
        {
            try
            {
                await answerRepository.UpdateAnswerAsync(new AnswerInfo()
                {
                    Content = content
                });
            }
            catch (Repository.Exceptions.AnswerNotExistException e)
            {
                throw new Exceptions.AnswerNotExistException(aid, e);
            }
        }

        public async Task ModifyTagAsync(int tid, string name, string desc)
        {
            try
            {
                await tagRepostory.UpdateTagAsync(new TagInfo()
                {
                    Name = name,
                    Desc = desc
                });
            }
            catch (Repository.Exceptions.TagNameHasExistException e)
            {
                throw new Exceptions.TagNameExistException(name, e);
            }
        }

        public async Task DeleteAnswerAsync(int aid)
        {
            var answer = await answerRepository.SelectAnswerByIdAsync(aid);
            if (answer == null)
            {
                throw new Exceptions.AnswerNotExistException(aid);
            }
            await answerRepository.DeleteAnswerByIdAsync(aid);
        }

        public async Task DeleteQuestionAsync(int qid)
        {
            var question = await questionRepository.SelectQuestionByIdAsync(qid);
            if (question == null)
            {
                throw new Exceptions.QuestionNotExistException(qid);
            }
            await questionRepository.DeleteQuestionByIdAsync(qid);
        }

        public async Task DeleteTagAsync(int tid)
        {
            var tag = await tagRepostory.SelectTagByIdAsync(tid);
            if (tag == null)
            {
                throw new Exceptions.TagNotExistException(tid);
            }
            await tagRepostory.DeleteTagAsync(tid);
        }
    }
}
