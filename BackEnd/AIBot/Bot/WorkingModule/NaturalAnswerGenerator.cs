using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Buaa.AIBot.Repository.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Buaa.AIBot.Services;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public class NaturalAnswerGenerator
    {
        private DatabaseContext context;
        private INLPService nlpService;
        private CancellationToken cancellationToken;

        public NaturalAnswerGenerator(DatabaseContext context, INLPService nlpService, Utils.GlobalCancellationTokenSource gcts)
        {
            this.context = context;
            this.cancellationToken = gcts.Token;
            this.nlpService = nlpService;
        }

        public async Task AddQuestionAndAnswersAsync(IEnumerable<string> questions, IEnumerable<string> answers)
        {
            var questionInfos = questions.Select(content => new NaturalQuestion() 
            { 
                Content = content,
                // NatrualQuestionAnswerRelations = new List<NatrualQuestionAnswerRelation>()
            }).ToList();
            var answerInfos = answers.Select(content => new NatrualAnswer() 
            { 
                Content = content,
                // NatrualQuestionAnswerRelations = new List<NatrualQuestionAnswerRelation>() 
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
                    // a.NatrualQuestionAnswerRelations.Add(r);
                    // q.NatrualQuestionAnswerRelations.Add(r);
                }
            }
            context.AddRange(questionInfos);
            context.AddRange(answerInfos);
            context.AddRange(relations);
            await context.SaveChangesAsync(cancellationToken);
            foreach (var q in questionInfos)
            {
                await nlpService.AddAsync(-q.NaturalQuestionId, q.Content, NLPService.Languages.Natrual);
            }
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
            // TODO
            Random rnd = new Random();
            int aid = answers[rnd.Next(answers.Count)];
            var ret = await context
                .NatrualAnswers
                .Where(na => na.NatrualAnswerId == qid)
                .Select(na => na.Content)
                .SingleAsync(cancellationToken);
            return ret;
        }
    }
}