using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz_App.Data;
using Quiz_App.DTOs;
using Quiz_App.Models;

namespace Quiz_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public QuestionController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            var questionDto = new QuestionDTO
            {
                Id = question.Id,
                Text = question.Text,
                Answers = question.Answers.Select(a => new AnswerDTO
                {
                    Id = a.Id,
                    Text = a.Text
                }).ToList()
            };

            return Ok(questionDto);
        }

        // POST: api/Question/SubmitAnswer
        [HttpPost("SubmitAnswer")]
        public async Task<ActionResult<AnswerResponseDTO>> SubmitAnswer(SubmitAnswerDTO answerDto)
        {
            // Find the quiz attempt
            var quizAttempt = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .ThenInclude(q => q.Questions)
                .FirstOrDefaultAsync(qa => qa.QuizId == answerDto.QuizId && qa.EndTime == null);

            if (quizAttempt == null)
            {
                return NotFound("Active quiz attempt not found");
            }

            // Find the question and selected answer
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == answerDto.QuestionId);

            if (question == null)
            {
                return NotFound("Question not found");
            }

            var selectedAnswer = question.Answers
                .FirstOrDefault(a => a.Id == answerDto.AnswerId);

            if (selectedAnswer == null)
            {
                return NotFound("Answer not found");
            }

            // Create a user response
            var userResponse = new UserResponse
            {
                QuizAttemptId = quizAttempt.Id,
                QuestionId = question.Id,
                AnswerId = selectedAnswer.Id,
                IsCorrect = selectedAnswer.IsCorrect,
                ResponseTime = DateTime.UtcNow
            };

            _context.UserResponses.Add(userResponse);
            await _context.SaveChangesAsync();

            // Determine if this is the last question
            bool isLastQuestion = false;
            int? nextQuestionId = null;

            var answeredQuestionIds = await _context.UserResponses
                .Where(ur => ur.QuizAttemptId == quizAttempt.Id)
                .Select(ur => ur.QuestionId)
                .ToListAsync();

            var nextQuestion = quizAttempt.Quiz.Questions
                .Where(q => !answeredQuestionIds.Contains(q.Id))
                .OrderBy(q => q.Id)
                .FirstOrDefault();

            if (nextQuestion == null)
            {
                isLastQuestion = true;
                
                // If this is the last question, complete the quiz
                quizAttempt.EndTime = DateTime.UtcNow;
                
                // Calculate score
                var totalQuestions = quizAttempt.Quiz.Questions.Count;
                var correctAnswers = await _context.UserResponses
                    .Where(ur => ur.QuizAttemptId == quizAttempt.Id && ur.IsCorrect)
                    .CountAsync();
                
                quizAttempt.Score = totalQuestions > 0 
                    ? (int)Math.Round((double)correctAnswers / totalQuestions * 100)
                    : 0;
                
                await _context.SaveChangesAsync();
            }
            else
            {
                nextQuestionId = nextQuestion.Id;
            }

            var feedbackMessage = selectedAnswer.IsCorrect
                ? "Correct answer!"
                : "Sorry, that's incorrect.";

            var response = new AnswerResponseDTO
            {
                IsCorrect = selectedAnswer.IsCorrect,
                FeedbackMessage = feedbackMessage,
                IsQuizComplete = isLastQuestion,
                NextQuestionId = nextQuestionId
            };

            return Ok(response);
        }
    }
} 