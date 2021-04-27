using Buaa.AIBot.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Repository.Implement
{
    public class AnswerRepository : IAnswerRepository
    {
        public Task DeleteAnswerByIdAsync(int answerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAnswerAsync(AnswerInfo answer)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerInfo> SelectAnswerByIdAsync(int answerId)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerInfo> SelectAnswerByQuestionAndUserAsync(int questionId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAnswerAsync(AnswerInfo answer)
        {
            throw new NotImplementedException();
        }
    }
}
