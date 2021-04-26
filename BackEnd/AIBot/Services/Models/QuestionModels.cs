using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Services.Models
{
    public class QuestionInformation
    {
        public string Title { get; set; }
        public string Remarks { get; set; }
        public int? Creater { get; set; }
        public int? Best { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public IEnumerable<int> Tags { get; set; }
        public IEnumerable<int> Answers { get; set; }
    }

    public class AnswerInformation
    {
        public string Content { get; set; }
        public int? Creater { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }

    public class TagInformation
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }

    public class QuestionModifyItems
    {
        public string Title { get; set; } = null;
        public string Remarks { get; set; } = null;
        public int? BestAnswer { get; set; } = null;
        public IEnumerable<int> Tags { get; set; } = null;
    }
}
