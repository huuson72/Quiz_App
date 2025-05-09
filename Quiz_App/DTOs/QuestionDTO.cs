using System.Collections.Generic;

namespace Quiz_App.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<AnswerDTO> Answers { get; set; } = new List<AnswerDTO>();
    }
} 