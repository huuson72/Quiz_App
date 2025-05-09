using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_App.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        
        // Indicates if this is the correct answer
        public bool IsCorrect { get; set; }
        
        // Foreign key
        public int QuestionId { get; set; }
        
        // Navigation property
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
    }
} 