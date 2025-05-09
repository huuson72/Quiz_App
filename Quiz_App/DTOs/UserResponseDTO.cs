namespace Quiz_App.DTOs
{
    public class SubmitAnswerDTO
    {
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
    
    public class AnswerResponseDTO
    {
        public bool IsCorrect { get; set; }
        public string FeedbackMessage { get; set; } = string.Empty;
        public bool IsQuizComplete { get; set; }
        public int? NextQuestionId { get; set; }
    }
} 