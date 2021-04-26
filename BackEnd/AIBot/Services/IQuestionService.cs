using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Services.Models;
using Buaa.AIBot.Services.Exceptions;

namespace Buaa.AIBot.Services
{
    interface IQuestionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="QuestionNotExistException"></exception>
        /// <param name="qid"></param>
        /// <returns></returns>
        Task<QuestionInformation> GetQuestionAsync(int qid);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AnswerNotExistException"></exception>
        /// <param name="qid"></param>
        /// <returns></returns>
        Task<AnswerInformation> GetAnswerAsync(int qid);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="TagNotExistException"></exception>
        /// <param name="tid"></param>
        /// <returns></returns>
        Task<TagInformation> GetTagAsync(int tid);

        Task<IEnumerable<int>> GetQuestionListAsync(IEnumerable<int> tags, int? pt, int number);

        Task<Dictionary<string, int>> GetTagListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="QuestionTitleTooLongException"></exception>
        /// <exception cref="UserNotExistException">creater not exist</exception>
        /// <exception cref="TagNotExistException"></exception>
        /// <param name="creater">uid</param>
        /// <param name="title"></param>
        /// <param name="remarks"></param>
        /// <param name="tags">tids</param>
        /// <returns>qid</returns>
        Task<int> AddQuestionAsync(int creater, string title, string remarks, IEnumerable<int> tags);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="UserHasAnswerTheQuestionException"></exception>
        /// <exception cref="UserNotExistException">creater not exist</exception>
        /// <param name="creater">uid</param>
        /// <param name="qid"></param>
        /// <param name="content"></param>
        /// <returns>aid</returns>
        Task<int> AddAnswerAsync(int creater, int qid, string content);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="TagNameTooLongException"></exception>
        /// <exception cref="TagNameExistException"></exception>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns>tid</returns>
        Task<int> AddTagAsync(string name, string content);

        /// <summary>
        /// qid is required. For other params, null mean no change.
        /// </summary>
        /// <exception cref="QuestionNotExistException"></exception>
        /// <exception cref="QuestionTitleTooLongException"></exception>
        /// <exception cref="TagNotExistException">If Tags is not null and there is a tid in the list that no tag match it</exception>
        /// <exception cref="AnswerNotExistException">If BestAnswer is not null and no answer match it. </exception>
        /// <param name="qid"></param>
        /// <param name="modifyItems"></param>
        /// <returns></returns>
        Task ModifyQuestionAsync(int qid, QuestionModifyItems modifyItems);

        /// <summary>
        /// aid is required. For other params, null mean no change.
        /// </summary>
        /// <exception cref="AnswerNotExistException"></exception>
        /// <param name="aid"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task ModifyAnswerAsync(int aid, string content);

        /// <summary>
        /// tid is required. For other params, null mean no change.
        /// </summary>
        /// <exception cref="TagNotExistException"></exception>
        /// <exception cref="TagNameTooLongException"></exception>
        /// <exception cref="TagNameExistException"></exception>
        /// <param name="tid"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        Task ModifyTagAsync(int tid, string name, string desc);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="QuestionNotExistException"></exception>
        /// <param name="qid"></param>
        /// <returns></returns>
        Task DeleteQuestionAsync(int qid);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AnswerNotExistException"></exception>
        /// <param name="aid"></param>
        /// <returns></returns>
        Task DeleteAnswerAsync(int aid);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="TagNotExistException"></exception>
        /// <param name="tid"></param>
        /// <returns></returns>
        Task DeleteTagAsync(int tid);
    }
}
