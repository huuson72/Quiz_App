namespace Quiz_App.DTOs
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
    
    // Only used internally when creating answers or retrieving answer details for review
    internal class AnswerDetailsDTO : AnswerDTO
    {
        public bool IsCorrect { get; set; }
    }
} 