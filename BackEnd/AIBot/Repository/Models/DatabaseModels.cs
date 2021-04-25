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
        [Key]
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

        // relation references
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public int? BestAnswerId { get; set; }
        public int? UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Remarks { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyTime { get; set; }

        // relation references
        public User User { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        public int? UserId { get; set; }
        public int QuestionId { get; set; }

        [Required]
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyTime { get; set; }


        // relation references
        public User User { get; set; }
        public Question Question { get; set; }
    }

    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        // relation references
        public List<Question> Questions { get; set; }
    }
}
