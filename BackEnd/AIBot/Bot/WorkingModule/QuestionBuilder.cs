using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;
using Buaa.AIBot.Services;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public class QuestionBuilder
    {
        public static readonly string SimpleDescribe = "SimpleDescribe";
        public static readonly string DetailDescribe = "DetailDescribe";
        public static readonly string OS = "OperatingSystem";
        public static readonly string IDE = "IDE";

        private IQuestionService questionService;

        public QuestionBuilder(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        public async Task<Tuple<List<int>, string>> SearchStatusAsync(IBotStatusContainer status)
        {
            var tags = await questionService.GetTagListAsync();
            var tagRet = new List<int>();
            var remarksRet = status.Get<string>(DetailDescribe);
            foreach (var pair in tags)
            {
                // TODO
            }
            return Tuple.Create(tagRet, remarksRet);
        }

        public async Task<int> BuildAsync(IBotStatusContainer status)
        {
            int uid = status.UserId;
            string title = status.Get<string>(SimpleDescribe);
            var res = await SearchStatusAsync(status);
            var tags = res.Item1;
            var remarks = res.Item2;
            int qid = await questionService.AddQuestionAsync(uid, title, remarks, tags);
            return qid;
        }
    }
}
