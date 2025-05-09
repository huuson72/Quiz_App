using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quiz_App.ViewModels
{
    public class QuizListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuestionCount { get; set; }
    }

    public class QuizDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuestionCount { get; set; }
        public int PassingScore { get; set; }
    }

    public class StartQuizViewModel
    {
        [Required]
        public int QuizId { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
    }

    public class QuestionViewModel
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int Progress { get; set; }
        public int QuestionNumber { get; set; }
        public int TotalQuestions { get; set; }
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    }

    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class SubmitAnswerViewModel
    {
        [Required]
        public int AttemptId { get; set; }
        
        [Required]
        public int QuestionId { get; set; }
        
        [Required]
        public int AnswerId { get; set; }
        
        public bool ShowFeedback { get; set; } = true;
    }

    public class FeedbackViewModel
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int SelectedAnswerId { get; set; }
        public string SelectedAnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int? CorrectAnswerId { get; set; }
        public string? CorrectAnswerText { get; set; }
        public int? NextQuestionId { get; set; }
        public bool IsLastQuestion { get; set; }
    }

    public class QuizResultViewModel
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
        public List<QuestionResultViewModel> QuestionResults { get; set; } = new List<QuestionResultViewModel>();
    }

    public class QuestionResultViewModel
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