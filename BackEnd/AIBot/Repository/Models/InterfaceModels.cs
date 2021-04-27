using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Repository.Models
{
    public enum AuthLevel
    {
        None = 0,
        User = 1,
        Admin = 2
    }

    public class UserInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Bcrypt { get; set; }
        public string Name { get; set; }
        public AuthLevel Auth { get; set; }
    }

    public class QuestionInfo
    {
        public int QuestionId { get; set; }

        /// <summary>
        /// The best answer of this question. null means no answer has been marked as best.
        /// </summary>
        public int? BestAnswerId { get; set; }

        /// <summary>
        /// The creater of this question. null means the creater has been removed.
        /// </summary>
        public int? CreaterId { get; set; }
        public string Title { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }

    public class QuestionWithListTag : QuestionInfo
    {
        public IEnumerable<int> Tags { get; set; }
    }

    public class AnswerInfo
    {
        public int AnswerId { get; set; }

        /// <summary>
        /// The creater of this answer. null means the creater has been removed.
        /// </summary>
        public int? CreaterId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }

    public class TagInfo
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
