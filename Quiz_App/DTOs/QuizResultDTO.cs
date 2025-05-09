using System;
using System.Collections.Generic;

namespace Quiz_App.DTOs
{
    public class QuizResultDTO
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int Score { get; set; } // Percentage
        public bool IsPassed { get; set; }
        public int DurationInSeconds { get; set; }
        public string FormattedDuration { get; set; } = string.Empty; // Human-readable format
        public DateTime CompletedAt { get; set; }
        public List<QuestionResultDTO> QuestionResults { get; set; } = new List<QuestionResultDTO>();
    }
    
    public class QuestionResultDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int SelectedAnswerId { get; set; }
        public string SelectedAnswerText { get; set; } = string.Empty;
        public bool WasCorrect { get; set; }
        public int? CorrectAnswerId { get; set; }
        public string? CorrectAnswerText { get; set; }
    }
} 