﻿using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Buaa.AIBot.Repository.Exceptions;

namespace Buaa.AIBot.Repository.Implement
{
    /// <summary>
    /// Implement of <see cref="IQuestionRepository"/>.
    /// </summary>
    /// <remarks><seealso cref="IQuestionRepository"/></remarks>
    public class QuestionRepository : RepositoryBase , IQuestionRepository
    {
        public QuestionRepository(DatabaseContext context) : base(context) { }

        public async Task<IEnumerable<int>> SelectAnswersForQuestionByIdAsync(int questionId)
        {
            var query = await Context
                .Answers
                .Where(a => a.QuestionId == questionId)
                .Select(a => a.AnswerId)
                .ToListAsync()
                ;
            if (query.Count != 0)
            {
                return query;
            }
            var question = await Context.Questions.FindAsync(questionId);
            if (question == null)
            {
                return null;
            }
            return query;
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
                if (tags != null)
                {
                    Requires = new HashSet<int>(tags);
                }
                else
                {
                    Requires = new HashSet<int>();
                }
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
            var list = tags.ToList();
            if (list.Count == 0)
            {
                return await Context.Questions.Select(q => q.QuestionId).ToListAsync();
            }
            string create_set = "drop table if exists tids;\ncreate temporary table tids (tid int not null, primary key(tid));\n";
            string values = string.Join("),(", list);
            string insert_set = $"insert into tids values ({values});\n";
            string select =
                "select *\n" +
                "from Questions\n" +
                "where not exists\n" +
                "(\n\t" +
                    "select tid from tids\n\t" +
                    "where not exists\n\t" +
                    "(\n\t\t" +
                        "select *\n\t\t" +
                        "from QuestionTagRelations as qt\n\t\t" +
                        "where qt.QuestionId=Questions.QuestionId and qt.TagId=tids.tid\n\t" +
                    ")\n" +
                ");";
            string sql = create_set + insert_set + select;
            var query = Context
                .Questions
                .FromSqlRaw(sql)
                .AsEnumerable()
                .Select(q => q.QuestionId);

            //var query = Context
            //    .QuestionTagRelations
            //    .GroupBy(qt => qt.QuestionId)
            //    .Where(q => matcher.Match(q.Select(qt => qt.TagId)))
            //    .Select(q => q.First().QuestionId);
            return query.ToList();
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> SelectTagsForQuestionByIdAsync(int questionId)
        {
            var query = Context
                .QuestionTagRelations
                .Where(qt => qt.QuestionId == questionId)
                .Select(qt => new KeyValuePair<string, int>(qt.Tag.Name, qt.TagId));
            var list = await query.ToListAsync();
            if (list.Count != 0)
            {
                return list;
            }
            var question = await Context.Questions.FindAsync(questionId);
            if (question == null)
            {
                return null;
            }
            return list;
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
                int aid = (int)question.BestAnswerId;
                var answer = await Context.Answers.FindAsync(aid);
                if (answer == null || answer.QuestionId != question.QuestionId)
                {
                    throw new AnswerNotExistException(aid);
                }
            }
            if (question.Tags != null)
            {
                await CheckTags(matcher);
            }
        }

        private class QuestionTagRelationPool
        {
            public Dictionary<int, QuestionTagRelation> Dict { get; } = new Dictionary<int, QuestionTagRelation>();
            private int qid;

            public QuestionTagRelationPool(int qid)
            {
                this.qid = qid;
            }

            public QuestionTagRelation Get(int tid)
            {
                if (Dict.TryGetValue(tid, out QuestionTagRelation ret))
                {
                    return ret;
                }
                ret = new QuestionTagRelation() { QuestionId = qid, TagId = tid };
                Dict[tid] = ret;
                return ret;
            }

            public void AddRange(IEnumerable<QuestionTagRelation> qts)
            {
                foreach (var qt in qts)
                {
                    Dict.TryAdd(qt.TagId, qt);
                }
            }
        }

        public async Task UpdateQuestionAsync(QuestionWithListTag question)
        {
            var target = await Context.Questions.FindAsync(question.QuestionId);
            if (target == null)
            {
                throw new QuestionNotExistException(question.QuestionId);
            }
            bool success = true;
            var matcher = new TagMatcher(question.Tags);
            if (question.Title != null)
            {
                success = false;
                if (question.Title.Length > Constants.QuestionTitleMaxLength)
                {
                    throw new QuestionTitleTooLongException(question.Title.Length, Constants.QuestionTitleMaxLength);
                }
                target.Title = question.Title;
            }
            if (question.Remarks != null)
            {
                success = false;
                target.Remarks = question.Remarks;
            }
            if (question.BestAnswerId != null)
            {
                success = false;
                target.BestAnswerId = question.BestAnswerId;
            }
            HashSet<int> tidsAdded = null;
            HashSet<int> tidsRemoved = null;
            QuestionTagRelationPool pool = null;
            if (question.Tags != null)
            {
                success = false;
                tidsAdded = new HashSet<int>();
                tidsRemoved = new HashSet<int>();
                pool = new QuestionTagRelationPool(question.QuestionId);
            }
            await CheckUpdateAsync(target, question, matcher);
            while (!success)
            {
                if (question.Tags != null)
                {
                    var tagsInDb = await Context.QuestionTagRelations
                        .Where(qt => qt.QuestionId == question.QuestionId)
                        .ToListAsync();
                    pool.AddRange(tagsInDb);
                    var inside = new HashSet<int>(tagsInDb.Select(qt => qt.TagId));
                    foreach (var tid in tidsAdded)
                    {
                        inside.Add(tid);
                    }
                    foreach (var tid in tidsRemoved)
                    {
                        inside.Remove(tid);
                    }
                    var toAdd = matcher.Requires
                        .Where(t => !inside.Contains(t))
                        .ToList();
                    var toRemove = inside
                        .Where(t => !matcher.Requires.Contains(t))
                        .ToList();
                    Context.QuestionTagRelations.AddRange(toAdd
                        .Select(t => pool.Get(t)));
                    Context.QuestionTagRelations.RemoveRange(toRemove
                        .Select(t => pool.Get(t)));
                    foreach (var tid in toAdd)
                    {
                        tidsAdded.Add(tid);
                        tidsRemoved.Remove(tid);
                    }
                    foreach (var tid in toRemove)
                    {
                        tidsRemoved.Add(tid);
                        tidsAdded.Remove(tid);
                    }
                }
                success = await TrySaveChangesAgainAndAgainAsync();
                if (success)
                {
                    break;
                }
                await CheckUpdateAsync(target, question, matcher);
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
