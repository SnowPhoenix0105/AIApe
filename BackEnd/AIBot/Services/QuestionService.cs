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
        private readonly ILikeRepository likeRepository;

        public QuestionService(IQuestionRepository questionRepository, IAnswerRepository answerRepository, ITagRepostory tagRepostory, ILikeRepository likeRepository)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.tagRepostory = tagRepostory;
            this.likeRepository = likeRepository;
        }

        public async Task<QuestionInformation> GetQuestionAsync(int qid, int? uid=null)
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
            bool? like = null;
            int likeNum;
            try
            {
                if (uid != null)
                {
                    try
                    {
                        bool res = await likeRepository.UserLikedQuestionAsync((int)uid, qid);
                        like = res;
                    }
                    catch (Repository.Exceptions.UserNotExistException)
                    {
                        like = null;
                    }
                }
                likeNum = await likeRepository.SelectLikesCountForQuestionAsync(qid);
            }
            catch (Repository.Exceptions.QuestionNotExistException e)
            {
                throw new Exceptions.QuestionNotExistException(qid, e);
            }
            return new QuestionInformation()
            {
                Title = question.Title,
                Remarks = question.Remarks,
                Creator = question.CreaterId,
                Like = like,
                LikeNum = likeNum,
                HotValue = question.HotValue,
                CreatTime = question.CreateTime,
                HotFreshTime = question.HotFreshTime,
                ModifyTime = question.ModifyTime,
                Tags = new Dictionary<string, int>(tags),
                Answers = answers
            };
        }

        public async Task<AnswerInformation> GetAnswerAsync(int aid, int? uid=null)
        {
            var answer = await answerRepository.SelectAnswerByIdAsync(aid);
            if (answer == null)
            {
                throw new Exceptions.AnswerNotExistException(aid);
            }
            bool? like = null;
            int likeNum;
            try
            {
                if (uid != null)
                {
                    try
                    {
                        bool res = await likeRepository.UserLikedAnswerAsync((int)uid, aid);
                        like = res;
                    }
                    catch (Repository.Exceptions.UserNotExistException)
                    {
                        like = null;
                    }
                }
                likeNum = await likeRepository.SelectLikesCountForAnswerAsync(aid);
            }
            catch (Repository.Exceptions.AnswerNotExistException e)
            {
                throw new Exceptions.AnswerNotExistException(aid, e);
            }
            return new AnswerInformation()
            {
                Content = answer.Content,
                Creator = answer.CreaterId,
                Like = like,
                LikeNum = likeNum,
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
                Category = tag.Category,
                Name = tag.Name,
                Desc = tag.Desc
            };
        }

        public async Task<IEnumerable<int>> GetQuestionListAsync(IEnumerable<int> tags, int? pt, int number)
        {
            if (number <= 0)
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
            catch (Repository.Exceptions.UserNotExistException e)
            {
                throw new Exceptions.UserNotExistException(creater, e);
            }
        }

        public async Task<int> AddTagAsync(string name, string desc, string category)
        {
            if (name.Length > Constants.TagNameMaxLength)
            {
                throw new Exceptions.TagNameTooLongException(name.Length, Constants.TagNameMaxLength);
            }
            if (!Enum.TryParse<TagCategory>(category, out var tagCategory))
            {
                category = category.Substring(0, 1).ToUpper() + category.Substring(1);
                if (!Enum.TryParse(category, out tagCategory))
                {
                    throw new UnknownTagCategoryException(category);
                }
            }
            if (tagCategory == TagCategory.None)
            {
                tagCategory = TagCategory.Other;
            }
            try
            {
                int tid = await tagRepostory.InsertTagAsync(new TagInfo()
                { 
                    Category = tagCategory,
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
            if (modifyItems.Title != null && modifyItems.Title.Length > Constants.QuestionTitleMaxLength)
            {
                throw new Exceptions.
                    QuestionTitleTooLongException(modifyItems.Title.Length, Constants.QuestionTitleMaxLength);
            }
            try
            {
                await questionRepository.UpdateQuestionAsync(new QuestionWithListTag()
                {
                    QuestionId = qid,
                    Title = modifyItems.Title,
                    Remarks = modifyItems.Remarks,
                    Tags = modifyItems.Tags
                });
            }
            catch (Repository.Exceptions.TagNotExistException e)
            {
                throw new Exceptions.TagNotExistException(e.TagId, e);
            }
            catch (Repository.Exceptions.QuestionNotExistException e)
            {
                throw new Exceptions.QuestionNotExistException(qid, e);
            }
            //catch (Repository.Exceptions.AnswerNotExistException e)
            //{
            //    if (modifyItems.BestAnswer != null)
            //    {
            //        throw new Exceptions.AnswerNotExistException((int)modifyItems.BestAnswer, e);
            //    }
            //    throw;
            //}
        }

        public async Task ModifyAnswerAsync(int aid, string content)
        {
            try
            {
                await answerRepository.UpdateAnswerAsync(new AnswerInfo()
                {
                    AnswerId = aid,
                    Content = content
                });
            }
            catch (Repository.Exceptions.AnswerNotExistException e)
            {
                throw new Exceptions.AnswerNotExistException(aid, e);
            }
        }

        public async Task ModifyTagAsync(int tid, string name, string desc, string category)
        {
            if (name != null)
            {
                int max = Constants.TagNameMaxLength;
                int actual = name.Length;
                if (actual > max)
                {
                    throw new Exceptions.TagNameTooLongException(actual, max);
                }
            }
            TagCategory tagCategory = TagCategory.None;
            if (category != null)
            {
                if (!Enum.TryParse<TagCategory>(category, out tagCategory))
                {
                    category = category.Substring(0, 1).ToUpper() + category.Substring(1);
                    if (!Enum.TryParse(category, out tagCategory))
                    {
                        throw new UnknownTagCategoryException(category);
                    }
                }
                if (tagCategory == TagCategory.None)
                {
                    tagCategory = TagCategory.Other;
                }
            }
            try
            {
                await tagRepostory.UpdateTagAsync(new TagInfo()
                {
                    TagId = tid,
                    Category = tagCategory,
                    Name = name,
                    Desc = desc
                });
            }
            catch (Repository.Exceptions.TagNotExistException e)
            {
                throw new Exceptions.TagNotExistException(tid, e);
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

        public async Task<LikeProduceResult> LikeQuestionAsync(int uid, int qid, bool target)
        {
            var ret = new LikeProduceResult();
            if (target)
            {
                try
                {
                    await likeRepository.InsertLikeForQuestionAsync(uid, qid);
                    ret.Status = LikeProduceResult.ResultStatus.success;
                }
                catch (Repository.Exceptions.QuestionNotExistException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.questionNotExist;
                }
                catch (Repository.Exceptions.UserHasLikedTargetException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.alreadyLiked;
                }
            }
            else
            {
                try
                {
                    await likeRepository.DeleteLikeForQuestionAsync(uid, qid);
                    ret.Status = LikeProduceResult.ResultStatus.success;
                }
                catch (Repository.Exceptions.QuestionNotExistException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.questionNotExist;
                }
                catch (Repository.Exceptions.UserNotLikedTargetException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.notLiked;
                }
            }
            ret.UserLiked = await likeRepository.UserLikedQuestionAsync(uid, qid);
            ret.LikeNum = await likeRepository.SelectLikesCountForQuestionAsync(qid);
            return ret;
        }

        public async Task<LikeProduceResult> LikeAnswerAsync(int uid, int aid, bool target)
        {
            var ret = new LikeProduceResult();
            if (target)
            {
                try
                {
                    await likeRepository.InsertLikeForAnswerAsync(uid, aid);
                    ret.Status = LikeProduceResult.ResultStatus.success;
                }
                catch (Repository.Exceptions.QuestionNotExistException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.answerNotExist;
                }
                catch (Repository.Exceptions.UserHasLikedTargetException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.alreadyLiked;
                }
            }
            else
            {
                try
                {
                    await likeRepository.DeleteLikeFroAnswerAsync(uid, aid);
                    ret.Status = LikeProduceResult.ResultStatus.success;
                }
                catch (Repository.Exceptions.AnswerNotExistException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.answerNotExist;
                }
                catch (Repository.Exceptions.UserNotLikedTargetException)
                {
                    ret.Status = LikeProduceResult.ResultStatus.notLiked;
                }
            }
            ret.UserLiked = await likeRepository.UserLikedAnswerAsync(uid, aid);
            ret.LikeNum = await likeRepository.SelectLikesCountForAnswerAsync(aid);
            return ret;
        }
    }
}
