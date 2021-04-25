using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Buaa.AIBot.Repository.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Bcrypt { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public AuthLevel Auth { get; set; }

        public List<Question> Questions { get; set; }

        public List<Answer> Answers { get; set; }
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public int? BestAnswerId { get; set; }
        public int? UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Remarks { get; set; }

        public DateTime CreateTime { get; set; } 

        public DateTime ModifyTime { get; set; }


        public User User { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class Answer
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int? UserId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }


        public User User { get; set; }
        public Question Question { get; set; }
    }

    public class Tag
    {
        public int TagId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        public List<Question> Questions { get; set; }
    }
}
