using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_App.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        
        // Foreign key
        public int QuizId { get; set; }
        
        // Navigation property
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; } = null!;
        
        // Navigation property for answers
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        
        // Navigation property for user responses
        public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
} 