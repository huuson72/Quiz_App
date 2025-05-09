using System;

namespace Quiz_App.DTOs
{
    public class StartQuizDTO
    {
        public int QuizId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
    
    public class QuizAttemptDTO
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int? CurrentQuestionId { get; set; }
    }
} 