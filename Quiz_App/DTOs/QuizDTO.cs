using System.Collections.Generic;

namespace Quiz_App.DTOs
{
    public class QuizDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PassingScore { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
    }
    
    public class QuizListItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuestionCount { get; set; }
    }
} 