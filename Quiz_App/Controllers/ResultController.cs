using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz_App.Data;
using Quiz_App.DTOs;

namespace Quiz_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public ResultController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: api/Result/5
        [HttpGet("{quizAttemptId}")]
        public async Task<ActionResult<QuizResultDTO>> GetQuizResult(int quizAttemptId)
        {
            var quizAttempt = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .Include(qa => qa.UserResponses)
                    .ThenInclude(ur => ur.Question)
                .Include(qa => qa.UserResponses)
                    .ThenInclude(ur => ur.Answer)
                .FirstOrDefaultAsync(qa => qa.Id == quizAttemptId);

            if (quizAttempt == null)
            {
                return NotFound("Quiz attempt not found");
            }

            if (quizAttempt.EndTime == null)
            {
                return BadRequest("Quiz is not completed yet");
            }

            // Get all questions from this quiz
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizAttempt.QuizId)
                .Include(q => q.Answers)
                .ToListAsync();

            // Get correct answers for each question
            var correctAnswers = questions
                .ToDictionary(
                    q => q.Id,
                    q => q.Answers.FirstOrDefault(a => a.IsCorrect)
                );

            // Format duration in a human-readable way
            var duration = quizAttempt.EndTime.Value - quizAttempt.StartTime;
            string formattedDuration;
            
            if (duration.TotalHours >= 1)
            {
                formattedDuration = $"{Math.Floor(duration.TotalHours)}h {duration.Minutes}m {duration.Seconds}s";
            }
            else if (duration.TotalMinutes >= 1)
            {
                formattedDuration = $"{duration.Minutes}m {duration.Seconds}s";
            }
            else
            {
                formattedDuration = $"{duration.Seconds}s";
            }

            // Create question result DTOs
            var questionResults = new List<QuestionResultDTO>();
            foreach (var response in quizAttempt.UserResponses)
            {
                var correctAnswer = correctAnswers[response.QuestionId];
                
                questionResults.Add(new QuestionResultDTO
                {
                    QuestionId = response.QuestionId,
                    QuestionText = response.Question.Text,
                    SelectedAnswerId = response.AnswerId,
                    SelectedAnswerText = response.Answer.Text,
                    WasCorrect = response.IsCorrect,
                    CorrectAnswerId = correctAnswer?.Id,
                    CorrectAnswerText = correctAnswer?.Text
                });
            }

            var quizResultDto = new QuizResultDTO
            {
                QuizId = quizAttempt.QuizId,
                QuizTitle = quizAttempt.Quiz.Title,
                TotalQuestions = questions.Count,
                CorrectAnswers = quizAttempt.UserResponses.Count(ur => ur.IsCorrect),
                Score = quizAttempt.Score ?? 0,
                IsPassed = quizAttempt.IsPassed,
                DurationInSeconds = (int)duration.TotalSeconds,
                FormattedDuration = formattedDuration,
                CompletedAt = quizAttempt.EndTime.Value,
                QuestionResults = questionResults
            };

            return Ok(quizResultDto);
        }
    }
} 