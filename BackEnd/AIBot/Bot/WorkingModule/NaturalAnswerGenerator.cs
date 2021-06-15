using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Buaa.AIBot.Repository.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Buaa.AIBot.Services;
using Microsoft.Extensions.Logging;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public class NaturalAnswerGenerator
    {
        private DatabaseContext context;
        private INLPService nlpService;
        private CancellationToken cancellationToken;
        private ILogger<NaturalAnswerGenerator> logger;

        public NaturalAnswerGenerator(DatabaseContext context, INLPService nlpService, Utils.GlobalCancellationTokenSource gcts, ILogger<NaturalAnswerGenerator> logger)
        {
            this.context = context;
            this.cancellationToken = gcts.Token;
            this.nlpService = nlpService;
            this.logger = logger;
        }

        public async Task AddQuestionAndAnswersAsync(IEnumerable<string> questions, IEnumerable<string> answers)
        {
            var questionInfos = questions.Select(content => new NaturalQuestion() 
            { 
                Content = content,
                NatrualQuestionAnswerRelations = new List<NatrualQuestionAnswerRelation>()
            }).ToList();
            var answerInfos = answers.Select(content => new NatrualAnswer() 
            { 
                Content = content,
                NatrualQuestionAnswerRelations = new List<NatrualQuestionAnswerRelation>() 
            }).ToList();
            var relations = new List<NatrualQuestionAnswerRelation>();
            foreach (var q in questionInfos)
            {
                foreach (var a in answerInfos)
                {
                    var r = new NatrualQuestionAnswerRelation()
                    {
                        NatrualAnswer = a,
                        NaturalQuestion = q
                    };
                    a.NatrualQuestionAnswerRelations.Add(r);
                    q.NatrualQuestionAnswerRelations.Add(r);
                }
            }
            context.NatrualQuestions.AddRange(questionInfos);
            context.NatrualAnswers.AddRange(answerInfos);
            context.NatrualQuestionAnswerRelations.AddRange(relations);
            await context.SaveChangesAsync(cancellationToken);
            foreach (var q in questionInfos)
            {
                await nlpService.AddAsync(-q.NaturalQuestionId, q.Content, NLPService.Languages.Natrual);
            }
        }

        private string GenerateDate()
        {
            var date = DateTime.Now;
            return $"现在是北京时间{date.Year}年{date.Month}月{date.Day}日{date.Hour}时{date.Minute}分{date.Second}秒哟{Kaomojis.Happy}";
        }

        private async Task<string> FinalProduceAsync(string origin)
        {
            if (origin == "[call date]")
            {
                return GenerateDate();
            }
            origin = origin
                .Replace("[Happy]", Kaomojis.Happy)
                .Replace("[Sad]", Kaomojis.Sad)
                .Replace("[Cute]", Kaomojis.Cute)
                ;
            return origin;
        }

        public async Task<string> GetAnswerAsync(int qid)
        {
            if (qid < 0)
            {
                qid = -qid;
            }
            var answers = await context
                .NatrualQuestionAnswerRelations
                .Where(nqa => nqa.NaturalQuestionId == qid)
                .Select(nqa => nqa.NatrualAnswerId)
                .ToListAsync(cancellationToken);
            if (answers.Count == 0)
            {
                return "小猿出了点问题呜呜呜";
            }
            logger.LogInformation("qid={qid}, answers={aids}", qid, answers);
            Random rnd = new Random();
            int aid = answers[rnd.Next(answers.Count)];
            var ret = await context
                .NatrualAnswers
                .Where(na => na.NatrualAnswerId == aid)
                .Select(na => na.Content)
                .SingleAsync(cancellationToken);
            return await FinalProduceAsync(ret);
        }
    }
}