using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_App.Models
{
    public class QuizAttempt
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        // Foreign key
        public int QuizId { get; set; }
        
        // Navigation property
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; } = null!;
        
        // Quiz statistics
        [Required]
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        // Calculated duration in seconds
        [NotMapped]
        public int Duration => EndTime.HasValue 
            ? (int)(EndTime.Value - StartTime).TotalSeconds 
            : 0;
        
        // Score as percentage
        public int? Score { get; set; }
        
        // Indicates if the quiz was passed based on passing criteria
        [NotMapped]
        public bool IsPassed => Score.HasValue && Quiz != null && Score.Value >= Quiz.PassingScore;
        
        // Navigation property for user responses
        public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
} 