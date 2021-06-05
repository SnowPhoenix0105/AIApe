using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Buaa.AIBot.Repository.Models;
using Buaa.AIBot.Utils;
using static Buaa.AIBot.Utils.QuestionJudgement;

namespace AIBotTest.Utils
{
    public class QuestionJudgmentTest
    {
        [Fact]
        public void Simple()
        {
            var tags = new Dictionary<TagCategory, IEnumerable<int>>()
            {
                [TagCategory.Lang] = new int[] { 1, 2, 3 },
                [TagCategory.Env] = new int[] { 4, 5, 6 },
                [TagCategory.Other] = new int[] { 7, 8, 9 }
            };

            var questions = new List<IQuestionTagInfo>()
            {
                new QuestionTagInfo()
                {
                    Qid = 1,
                    Tags = new Dictionary<TagCategory, IEnumerable<int>>()
                    {
                        [TagCategory.Lang] = new int[]{ },
                        [TagCategory.Env] = new int[]{ 4, },
                        [TagCategory.Other] = new int[]{ 7, },

                    }
                },
                new QuestionTagInfo()
                {
                    Qid = 2,
                    Tags = new Dictionary<TagCategory, IEnumerable<int>>()
                    {
                        [TagCategory.Lang] = new int[]{ },
                        [TagCategory.Env] = new int[]{ 5, },
                        [TagCategory.Other] = new int[]{ 7, },

                    }
                },
                new QuestionTagInfo()
                {
                    Qid = 3,
                    Tags = new Dictionary<TagCategory, IEnumerable<int>>()
                    {
                        [TagCategory.Lang] = new int[]{ },
                        [TagCategory.Env] = new int[]{ },
                        [TagCategory.Other] = new int[]{ 7, },
                    }
                },
            };

            var selected = new Dictionary<TagCategory, IEnumerable<int>>()
            {
                [TagCategory.Lang] = new int[] { },
                [TagCategory.Env] = new int[] { 4, 5 },
                [TagCategory.Other] = new int[] { 7 }
            };

            var res = QuestionJudgement.GetFilteredQuestions(questions, selected);
            Assert.Contains(1, res);
            Assert.Contains(2, res);
            Assert.DoesNotContain(3, res);
        }
    }
}
