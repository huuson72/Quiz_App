using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quiz_App.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        // Minimum score percentage to pass the quiz
        public int PassingScore { get; set; } = 60;
        
        // Navigation property
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
} 