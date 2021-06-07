using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Services;
using Buaa.AIBot.Repository.Models;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public class InnerRepoSearcher
    {
        private readonly IQuestionService questionService;
        private readonly INLPService nlpService;
        private readonly double minScore;

        public InnerRepoSearcher(IQuestionService questionService, INLPService nlpService, double minScore)
        {
            this.questionService = questionService;
            this.nlpService = nlpService;
            this.minScore = minScore;
        }

        public class QuestionScoreInfo
        {
            public int Qid { get; set; }
            public double Score { get; set; }
            public IReadOnlyDictionary<TagCategory, IReadOnlyDictionary<int, string>> Tags { get; set; }
        }

        private async Task<IReadOnlyDictionary<TagCategory, IReadOnlyDictionary<int, string>>> BuildTagsAsync(IReadOnlyDictionary<int, TagCategory> tagIndex, int qid)
        {
            var questionTags = await questionService.QuestionRepository.SelectTagsForQuestionByIdAsync(qid);
            if (questionTags == null)
            {
                return null;
            }
            var ret = new Dictionary<TagCategory, Dictionary<int, string>>(
                Enum.GetValues<TagCategory>().Select(c => new KeyValuePair<TagCategory, Dictionary<int, string>>(c, new Dictionary<int, string>())));
            foreach (var kv in questionTags)
            {
                var category = tagIndex[kv.Value];
                ret[category][kv.Value] = kv.Key;
            }
            return new Dictionary<TagCategory, IReadOnlyDictionary<int, string>>(
                ret.Select(kv => new KeyValuePair<TagCategory, IReadOnlyDictionary<int, string>>(kv.Key, kv.Value)));
        }

        public async Task<IEnumerable<QuestionScoreInfo>> SearchAsync(string question)
        {
            var res = await nlpService.RetrievalAsync(question, 30, Enum.GetValues<NLPService.Languages>().ToList());
            var tagIndex = await questionService.GetTagCategoryIndexAsync();
            var ret = new List<QuestionScoreInfo>();
            foreach (var t in res)
            {
                if (t.Item2 < minScore)
                {
                    continue;
                }
                var tags = await BuildTagsAsync(tagIndex, t.Item1);
                if (tags != null)
                {
                ret.Add(new QuestionScoreInfo()
                    {
                        Qid = t.Item1,
                        Score = t.Item2,
                        Tags = tags
                    });
                }
            }
            return ret;
        }
    }
}
