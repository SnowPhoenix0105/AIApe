using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Buaa.AIBot.Repository.Implement
{
    public class QuestionRepository : RepositoryBase , IQuestionRepository
    {
        public QuestionRepository(DatabaseContext context) : base(context) { }

        public async Task<IEnumerable<int>> SelectAnswersForQuestionByIdAsync(int questionId)
        {
            var question = await Context.Questions.FindAsync(questionId);
            if (question == null)
            {
                return null;
            }
            var query = question
                .Answers
                .Select(a => a.AnswerId);
            return query.ToList();
        }

        public async Task<QuestionInfo> SelectQuestionByIdAsync(int questionId)
        {
            var question = await Context
                .Questions
                .Select(q => new QuestionInfo()
                {
                    QuestionId = q.QuestionId,
                    BestAnswerId = q.BestAnswerId,
                    CreaterId = q.UserId,
                    Title = q.Title,
                    Remarks = q.Remarks,
                    CreateTime = q.CreateTime,
                    ModifyTime = q.ModifyTime
                })
                .Where(q => q.QuestionId == questionId)
                .SingleOrDefaultAsync();
            return question;
        }

        // TODO can make it faster?
        private class TagMatcher
        {
            public IReadOnlySet<int> Requires { get; }

            public TagMatcher(IEnumerable<int> tags)
            {
                Requires = new HashSet<int>(tags);
            }

            public bool Match(IEnumerable<int> actualTags)
            {
                switch (Requires.Count)
                {
                    case 0:
                        return true;
                    case 1:
                        int target = Requires.First();
                        foreach (var tid in actualTags)
                        {
                            if (tid == target)
                            {
                                return true;
                            }
                        }
                        return false;
                    default:
                        break;
                }
                int remain = Requires.Count;
                foreach (var tid in actualTags)
                {
                    if (Requires.Contains(tid))
                    {
                        remain--;
                        if (remain == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public async Task<IEnumerable<int>> SelectQuestionsByTagsAsync(IEnumerable<int> tags)
        {
            var matcher = new TagMatcher(tags);
            var query = Context
                .QuestionTagRelations
                .GroupBy(qt => qt.QuestionId)
                .Where(q => matcher.Match(q.Select(qt => qt.TagId)))
                .Select(q => q.First().QuestionId);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> SelectTagsForQuestionByIdAsync(int questionId)
        {
            var query = Context
                .QuestionTagRelations
                .Where(qt => qt.QuestionId == questionId)
                .Select(qt => new KeyValuePair<string, int>(qt.Tag.Name, qt.TagId));
            return await query.ToListAsync();
        }

        private async Task CheckInsertAsync(QuestionWithListTag question, TagMatcher matcher)
        {
            if ((await Context.Users.SingleOrDefaultAsync(u => u.UserId == question.CreaterId)) == null)
            {
                throw new UserNotExistException((int)question.CreaterId);
            }
            await CheckTags(matcher);
        }

        private async Task CheckTags(TagMatcher matcher)
        {
            if (!matcher.Match(Context.Tags.Select(t => t.TagId)))
            {
                var set = new HashSet<int>(matcher.Requires);
                foreach (int t in await Context.Tags.Select(t => t.TagId).ToListAsync())
                {
                    set.Remove(t);
                }
                int tid = set.FirstOrDefault();
                throw new TagNotExistException(tid);
            }
        }

        public async Task<int> InsertQuestionAsync(QuestionWithListTag question)
        {
            if (question.CreaterId == null)
            {
                throw new ArgumentNullException(nameof(question.CreaterId));
            }
            if (question.Title.Length > Constants.QuestionTitleMaxLength)
            {
                throw new QuestionTitleTooLongException(question.Title.Length, Constants.QuestionTitleMaxLength);
            }
            var matcher = new TagMatcher(question.Tags);
            await CheckInsertAsync(question, matcher);
            var target = new QuestionData()
            {
                UserId = question.CreaterId,
                Title = question.Title,
                Remarks = question.Remarks
            };
            Context.Questions.Add(target);
            bool success = false;
            while (!success)
            {
                success = await TrySaveChangesAgainAndAgainAsync();
                if (success)
                {
                    break;
                }
                await CheckInsertAsync(question, matcher);
            }
            int qid = target.QuestionId;
            Context.QuestionTagRelations.AddRange(
                matcher.Requires.Select(t => new QuestionTagRelation()
                {
                    QuestionId = qid,
                    TagId = t
                }));
            success = false;
            try
            {
                while (!success)
                {
                    success = await TrySaveChangesAgainAndAgainAsync();
                    if (success)
                    {
                        break;
                    }
                    await CheckTags(matcher);
                }
            }
            catch (Exception)
            {
                Context.Remove(target);
                await SaveChangesAgainAndAgainAsync();
            }
            return qid;
        }

        private async Task CheckUpdateAsync(QuestionData target, QuestionWithListTag question, TagMatcher matcher)
        {
            if ((await Context.Questions.FindAsync(question.QuestionId)) == null)
            {
                throw new QuestionNotExistException(question.QuestionId);
            }
            if (question.BestAnswerId != null)
            {
                if (target
                    .Answers
                    .Select(a => (int?)a.AnswerId)
                    .SingleOrDefault(aid => aid == question.BestAnswerId) == null)
                {
                    throw new AnswerNotExistException((int)question.BestAnswerId);
                }
            }
            if (question.Tags != null)
            {
                await CheckTags(matcher);
            }
        }

        public async Task UpdateQuestionAsync(QuestionWithListTag question)
        {
            var target = await Context.Questions.FindAsync(question.QuestionId);
            if (target == null)
            {
                throw new QuestionNotExistException(question.QuestionId);
            }
            if (question.Title != null)
            {
                if (question.Title.Length > Constants.QuestionTitleMaxLength)
                {
                    throw new QuestionTitleTooLongException(question.Title.Length, Constants.QuestionTitleMaxLength);
                }
                target.Title = question.Title;
            }
            if (question.Remarks != null)
            {
                target.Remarks = question.Remarks;
            }
            if (question.BestAnswerId != null)
            {
                target.BestAnswerId = question.BestAnswerId;
            }
            var matcher = new TagMatcher(question.Tags);
            await CheckUpdateAsync(target, question, matcher);
            Context.Questions.Add(target);
            bool success = false;
            while (!success)
            {
                success = await TrySaveChangesAgainAndAgainAsync();
                if (success)
                {
                    break;
                }
                await CheckUpdateAsync(target, question, matcher);
            }
            if (question.Tags != null)
            {
                int qid = target.QuestionId;
                Context.QuestionTagRelations.AddRange(
                    matcher
                    .Requires
                    .SkipWhile(t => target.QuestionTagRelation.Select(qt => qt.TagId).Contains(t))
                    .Select(t => new QuestionTagRelation()
                    {
                        QuestionId = qid,
                        TagId = t
                    }));
                success = false;
                try
                {
                    while (!success)
                    {
                        success = await TrySaveChangesAgainAndAgainAsync();
                        if (success)
                        {
                            break;
                        }
                        await CheckTags(matcher);
                    }
                }
                catch (Exception)
                {
                    Context.Remove(target);
                    await SaveChangesAgainAndAgainAsync();
                }
            }
        }

        public async Task DeleteQuestionByIdAsync(int questionId)
        {
            var target = await Context.Questions.FindAsync(questionId);
            if (target != null)
            {
                Context.Remove(target);
                await SaveChangesAgainAndAgainAsync();
            }
        }
    }
}
