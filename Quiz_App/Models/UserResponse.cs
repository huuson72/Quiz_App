using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_App.Models
{
    public class UserResponse
    {
        [Key]
        public int Id { get; set; }
        
        // Foreign keys
        public int QuizAttemptId { get; set; }
        
        public int QuestionId { get; set; }
        
        public int AnswerId { get; set; }
        
        // Navigation properties
        [ForeignKey("QuizAttemptId")]
        public virtual QuizAttempt QuizAttempt { get; set; } = null!;
        
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
        
        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; } = null!;
        
        // Indicates if the answer was correct
        public bool IsCorrect { get; set; }
        
        // Timestamp when the response was submitted
        public DateTime ResponseTime { get; set; }
    }
} 