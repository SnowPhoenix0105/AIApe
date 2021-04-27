using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Repository
{
    public class RepositoryException : Exception
    {
        public RepositoryException() : base() { }
        public RepositoryException(string msg) : base(msg) { }
        public RepositoryException(string msg, Exception innerException) : base(msg, innerException) { }
    }

    public class EmailHasExistException : RepositoryException
    {
        public EmailHasExistException(string email) 
            : base($"User with email={email} has already existed.") { }
    }

    public class NameHasExistException : RepositoryException
    {
        public NameHasExistException(string name) 
            : base($"User with name={name} has already existed.") { }
    }

    public class TagNameHasExistException : RepositoryException
    {
        public TagNameHasExistException(string name) 
            : base($"Tag with name={name} has already existed.") { }
    }

    public class UserNotExistException : RepositoryException
    {
        public UserNotExistException(int userId) 
            : base($"User with uid={userId} is not exist.") { }
    }

    public class TagNotExistException : RepositoryException
    {
        public int TagId { get; }

        public TagNotExistException(int tagId) 
            : base($"Tag with tid={tagId} is not exist.")
        {
            TagId = tagId;
        }
    }

    public class QuestionNotExistException : RepositoryException
    {
        public QuestionNotExistException(int questionId) 
            : base($"Question with qid={questionId} is not exist.") { }
    }

    public class UserHasAnswerTheQuestionException : RepositoryException
    {
        public UserHasAnswerTheQuestionException(int userId, int questionId)
            : base($"User with uid={userId} has answer the question with qid={questionId}") { }
    }

    public class AnswerNotExistException : RepositoryException
    {
        public AnswerNotExistException(int questionId, int answerId) 
            : base($"Answer with qid={questionId} and aid={answerId} is not exist.") { }
    }
}
