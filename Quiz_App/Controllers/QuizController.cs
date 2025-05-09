using System;
using System.Collections.Generic;
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
    public class QuizController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public QuizController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: api/Quiz
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizListItemDTO>>> GetQuizzes()
        {
            var quizzes = await _context.Quizzes
                .Select(q => new QuizListItemDTO
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    QuestionCount = q.Questions.Count
                })
                .ToListAsync();

            return Ok(quizzes);
        }

        // GET: api/Quiz/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDTO>> GetQuiz(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            var quizDto = new QuizDTO
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                PassingScore = quiz.PassingScore,
                Questions = quiz.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new AnswerDTO
                    {
                        Id = a.Id,
                        Text = a.Text
                    }).ToList()
                }).ToList()
            };

            return Ok(quizDto);
        }

        // POST: api/Quiz/Start
        [HttpPost("Start")]
        public async Task<ActionResult<QuizAttemptDTO>> StartQuiz(StartQuizDTO startQuizDto)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == startQuizDto.QuizId);

            if (quiz == null)
            {
                return NotFound("Quiz not found");
            }

            if (!quiz.Questions.Any())
            {
                return BadRequest("Quiz has no questions");
            }

            var quizAttempt = new QuizAttempt
            {
                QuizId = quiz.Id,
                UserId = startQuizDto.UserId,
                StartTime = DateTime.UtcNow
            };

            _context.QuizAttempts.Add(quizAttempt);
            await _context.SaveChangesAsync();

            return Ok(new QuizAttemptDTO
            {
                Id = quizAttempt.Id,
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                StartTime = quizAttempt.StartTime,
                CurrentQuestionId = quiz.Questions.FirstOrDefault()?.Id
            });
        }
    }
} 