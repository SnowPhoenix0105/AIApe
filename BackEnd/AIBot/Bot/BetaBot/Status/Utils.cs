using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.WorkingModule;
using Buaa.AIBot.Bot.Framework;
using Buaa.AIBot.Bot;
using Buaa.AIBot.Repository.Models;
using static Buaa.AIBot.Bot.WorkingModule.InnerRepoSearcher;

namespace Buaa.AIBot.Bot.BetaBot.Status
{
    public static class Utils
    {
        public static Task<IEnumerable<QuestionScoreInfo>> GetLocalSearchResultAsync(this IWorkingModule worker, string content)
        {
            var searcher = worker.GetInnerRepoSearcher();
            return searcher.SearchAsync(content);
        }

        public static Dictionary<TagCategory, HashSet<int>> GetSelectedTags(this IBotStatusContainer status)
        {
            Dictionary<TagCategory, HashSet<int>> ret;
            if (status.TryGet(Constants.Key.SelectedTags, out ret))
            {
                return ret;
            }
            ret = new Dictionary<TagCategory, HashSet<int>>(Enum.GetValues<TagCategory>().Select(c => new KeyValuePair<TagCategory, HashSet<int>>(c, new HashSet<int>())));
            status.Put(Constants.Key.SelectedTags, ret);
            return ret;
        }

        public static IEnumerable<QuestionScoreInfo> GetCheckingQuestions(this IBotStatusContainer status)
        {
            return status.Get<string>(Constants.Key.Checking) == Constants.Value.CheckingLong ?
                status.Get<IEnumerable<QuestionScoreInfo>>(Constants.Key.LongQuestion) :
                status.Get<IEnumerable<QuestionScoreInfo>>(Constants.Key.ShortQuestion);
        }

        private class QuestionTagAdapter : AIBot.Utils.QuestionJudgement.IQuestionTagInfo
        {
            public QuestionTagAdapter(QuestionScoreInfo origin)
            {
                Qid = origin.Qid;
                Tags = new Dictionary<TagCategory, IEnumerable<int>>(origin.Tags.Select(kv => new KeyValuePair<TagCategory, IEnumerable<int>>(kv.Key, kv.Value.Keys)));
            }
            public int Qid { get; }
            public IReadOnlyDictionary<TagCategory, IEnumerable<int>> Tags { get; }
        }

        /// <summary>
        /// <see cref="Constants.Key.Checking"/> in <paramref name="status"/> should be set.
        /// if <see cref="Constants.Key.Checking"/> is set to <see cref="Constants.Value.CheckingLong"/>, <see cref="Constants.Key.LongQuestion"/> is required;
        /// the same as short.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetSelectedQuestions(
            this IBotStatusContainer status,
            Dictionary<TagCategory, HashSet<int>> selectedTags = null,
            IEnumerable<QuestionScoreInfo> questions = null)
        {
            selectedTags ??= status.GetSelectedTags();
            questions ??= status.GetCheckingQuestions();
            var res = AIBot.Utils.QuestionJudgement.GetFilteredQuestions(
                questions.Select(q => new QuestionTagAdapter(q)).ToList(), 
                new Dictionary<TagCategory, IReadOnlyCollection<int>>(selectedTags.Select(kv => 
                    new KeyValuePair<TagCategory, IReadOnlyCollection<int>>(kv.Key, kv.Value))));
            return res;
        }

        public static IEnumerable<int> GetSortedSelectedQuestions(
            this IBotStatusContainer status,
            Dictionary<TagCategory, HashSet<int>> selectedTags = null,
            IEnumerable<QuestionScoreInfo> questions = null)
        {
            selectedTags ??= status.GetSelectedTags();
            questions ??= status.GetCheckingQuestions();
            var res = status.GetSelectedQuestions(selectedTags, questions);
            var set = new HashSet<int>(res);
            var query = from question in questions
                        where set.Contains(question.Qid)
                        orderby question.Score descending
                        select question.Qid;
            return query.ToList();
        }

        public static Dictionary<int, double> GetSelectedQuestionsWithScore(
            this IBotStatusContainer status,
            Dictionary<TagCategory, HashSet<int>> selectedTags = null,
            IEnumerable<QuestionScoreInfo> questions = null)
        {
            selectedTags ??= status.GetSelectedTags();
            questions ??= status.GetCheckingQuestions();
            var res = status.GetSelectedQuestions(selectedTags, questions);
            var set = new HashSet<int>(res);
            var query = from question in questions
                        where set.Contains(question.Qid)
                        select new KeyValuePair<int, double>(question.Qid, question.Score);
            return new Dictionary<int, double>(query);
        }
    }
}
