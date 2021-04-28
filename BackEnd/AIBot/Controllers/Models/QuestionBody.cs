using System.Collections.Generic;

namespace Buaa.AIBot.Controllers.Models
{
    public class QuestionBody
    {
        public IEnumerable<int> Tags { get; set; }

        public int? Pt { get; set; }

        public int? Number { get; set; }

        public string Title { get; set; }

        public string Remarks { get; set; }

        public int Qid { get; set; }

        public string Content { get; set; }   
    }
}