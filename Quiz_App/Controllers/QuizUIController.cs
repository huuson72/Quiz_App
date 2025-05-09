using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz_App.Data;
using Quiz_App.Models;
using Quiz_App.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz_App.Controllers
{
    public class QuizUIController : Controller
    {
        private readonly QuizDbContext _context;

        public QuizUIController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: QuizUI/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            var viewModel = new QuizDetailsViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                QuestionCount = quiz.Questions.Count,
                PassingScore = quiz.PassingScore
            };

            return View(viewModel);
        }

        // GET: QuizUI/Start/5
        public IActionResult Start(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new StartQuizViewModel
            {
                QuizId = id.Value,
                UserId = "user" + DateTime.Now.Ticks.ToString().Substring(0, 8) // Generate a simple user ID
            };

            return View(viewModel);
        }

        // POST: QuizUI/Start
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(StartQuizViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new quiz attempt
                var quizAttempt = new QuizAttempt
                {
                    QuizId = viewModel.QuizId,
                    UserId = viewModel.UserId,
                    StartTime = DateTime.UtcNow
                };

                _context.QuizAttempts.Add(quizAttempt);
                await _context.SaveChangesAsync();

                // Get the first question
                var firstQuestion = await _context.Questions
                    .Where(q => q.QuizId == viewModel.QuizId)
                    .OrderBy(q => q.Id)
                    .FirstOrDefaultAsync();

                if (firstQuestion == null)
                {
                    return NotFound("No questions found for this quiz.");
                }

                // Redirect to the question page
                return RedirectToAction("Question", new { attemptId = quizAttempt.Id, questionId = firstQuestion.Id });
            }

            return View(viewModel);
        }

        // GET: QuizUI/Question
        public async Task<IActionResult> Question(int attemptId, int questionId)
        {
            var quizAttempt = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .FirstOrDefaultAsync(qa => qa.Id == attemptId);

            if (quizAttempt == null)
            {
                return NotFound("Quiz attempt not found.");
            }

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                return NotFound("Question not found.");
            }

            // Check if the question belongs to the quiz
            if (question.QuizId != quizAttempt.QuizId)
            {
                return BadRequest("Question does not belong to this quiz.");
            }

            // Check if this question has already been answered
            var existingResponse = await _context.UserResponses
                .FirstOrDefaultAsync(ur => ur.QuizAttemptId == attemptId && ur.QuestionId == questionId);

            if (existingResponse != null)
            {
                // Find the next unanswered question
                var nextQuestion = await _context.Questions
                    .Where(q => q.QuizId == quizAttempt.QuizId)
                    .Where(q => !_context.UserResponses
                        .Where(ur => ur.QuizAttemptId == attemptId)
                        .Select(ur => ur.QuestionId)
                        .Contains(q.Id))
                    .OrderBy(q => q.Id)
                    .FirstOrDefaultAsync();

                if (nextQuestion != null)
                {
                    return RedirectToAction("Question", new { attemptId, questionId = nextQuestion.Id });
                }
                else
                {
                    // All questions answered, show results
                    return RedirectToAction("Results", new { attemptId });
                }
            }

            // Count total questions and answered questions
            var totalQuestions = await _context.Questions.CountAsync(q => q.QuizId == quizAttempt.QuizId);
            var answeredQuestions = await _context.UserResponses.CountAsync(ur => ur.QuizAttemptId == attemptId);

            var viewModel = new QuestionViewModel
            {
                AttemptId = attemptId,
                QuizId = quizAttempt.QuizId,
                QuizTitle = quizAttempt.Quiz.Title,
                QuestionId = question.Id,
                QuestionText = question.Text,
                Progress = (int)Math.Round((double)answeredQuestions / totalQuestions * 100),
                QuestionNumber = answeredQuestions + 1,
                TotalQuestions = totalQuestions,
                Answers = question.Answers.Select(a => new AnswerViewModel
                {
                    Id = a.Id,
                    Text = a.Text
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: QuizUI/SubmitAnswer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAnswer(SubmitAnswerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var question = await _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.Id == viewModel.QuestionId);

                if (question == null)
                {
                    return NotFound("Question not found.");
                }

                var selectedAnswer = question.Answers
                    .FirstOrDefault(a => a.Id == viewModel.AnswerId);

                if (selectedAnswer == null)
                {
                    return NotFound("Answer not found.");
                }

                // Create user response
                var userResponse = new UserResponse
                {
                    QuizAttemptId = viewModel.AttemptId,
                    QuestionId = viewModel.QuestionId,
                    AnswerId = viewModel.AnswerId,
                    IsCorrect = selectedAnswer.IsCorrect,
                    ResponseTime = DateTime.UtcNow
                };

                _context.UserResponses.Add(userResponse);
                await _context.SaveChangesAsync();

                // Find the next question
                var nextQuestion = await _context.Questions
                    .Where(q => q.QuizId == question.QuizId)
                    .Where(q => !_context.UserResponses
                        .Where(ur => ur.QuizAttemptId == viewModel.AttemptId)
                        .Select(ur => ur.QuestionId)
                        .Contains(q.Id))
                    .OrderBy(q => q.Id)
                    .FirstOrDefaultAsync();

                // Check if feedback is requested
                if (viewModel.ShowFeedback)
                {
                    // Redirect to feedback page
                    return RedirectToAction("Feedback", new
                    {
                        attemptId = viewModel.AttemptId,
                        questionId = viewModel.QuestionId,
                        answerId = viewModel.AnswerId,
                        nextQuestionId = nextQuestion?.Id
                    });
                }

                if (nextQuestion != null)
                {
                    // Go to next question
                    return RedirectToAction("Question", new { attemptId = viewModel.AttemptId, questionId = nextQuestion.Id });
                }
                else
                {
                    // Complete the quiz
                    var quizAttempt = await _context.QuizAttempts
                        .Include(qa => qa.Quiz)
                        .FirstOrDefaultAsync(qa => qa.Id == viewModel.AttemptId);

                    if (quizAttempt != null)
                    {
                        quizAttempt.EndTime = DateTime.UtcNow;

                        // Calculate score
                        var totalQuestions = await _context.Questions.CountAsync(q => q.QuizId == quizAttempt.QuizId);
                        var correctAnswers = await _context.UserResponses
                            .CountAsync(ur => ur.QuizAttemptId == viewModel.AttemptId && ur.IsCorrect);

                        quizAttempt.Score = totalQuestions > 0
                            ? (int)Math.Round((double)correctAnswers / totalQuestions * 100)
                            : 0;

                        await _context.SaveChangesAsync();
                    }

                    // Show results
                    return RedirectToAction("Results", new { attemptId = viewModel.AttemptId });
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Question", new { attemptId = viewModel.AttemptId, questionId = viewModel.QuestionId });
        }

        // GET: QuizUI/Feedback
        public async Task<IActionResult> Feedback(int attemptId, int questionId, int answerId, int? nextQuestionId)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                return NotFound();
            }

            var selectedAnswer = await _context.Answers.FindAsync(answerId);
            if (selectedAnswer == null)
            {
                return NotFound();
            }

            var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

            var viewModel = new FeedbackViewModel
            {
                AttemptId = attemptId,
                QuestionId = questionId,
                QuestionText = question.Text,
                SelectedAnswerId = answerId,
                SelectedAnswerText = selectedAnswer.Text,
                IsCorrect = selectedAnswer.IsCorrect,
                CorrectAnswerId = correctAnswer?.Id,
                CorrectAnswerText = correctAnswer?.Text,
                NextQuestionId = nextQuestionId,
                IsLastQuestion = nextQuestionId == null
            };

            return View(viewModel);
        }

        // GET: QuizUI/Results
        public async Task<IActionResult> Results(int attemptId)
        {
            var quizAttempt = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .Include(qa => qa.UserResponses)
                    .ThenInclude(ur => ur.Question)
                .Include(qa => qa.UserResponses)
                    .ThenInclude(ur => ur.Answer)
                .FirstOrDefaultAsync(qa => qa.Id == attemptId);

            if (quizAttempt == null)
            {
                return NotFound();
            }

            // Ensure the quiz is completed
            if (quizAttempt.EndTime == null)
            {
                quizAttempt.EndTime = DateTime.UtcNow;

                // Calculate score if not already calculated
                if (quizAttempt.Score == null)
                {
                    var totalQuestions = await _context.Questions.CountAsync(q => q.QuizId == quizAttempt.QuizId);
                    var correctAnswers = quizAttempt.UserResponses.Count(ur => ur.IsCorrect);

                    quizAttempt.Score = totalQuestions > 0
                        ? (int)Math.Round((double)correctAnswers / totalQuestions * 100)
                        : 0;

                    await _context.SaveChangesAsync();
                }
            }

            // Get all questions from this quiz
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizAttempt.QuizId)
                .Include(q => q.Answers)
                .ToListAsync();

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

            var viewModel = new QuizResultViewModel
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
                QuestionResults = quizAttempt.UserResponses.Select(ur => {
                    var correctAnswer = ur.Question.Answers.FirstOrDefault(a => a.IsCorrect);
                    return new QuestionResultViewModel
                    {
                        QuestionId = ur.QuestionId,
                        QuestionText = ur.Question.Text,
                        SelectedAnswerId = ur.AnswerId,
                        SelectedAnswerText = ur.Answer.Text,
                        WasCorrect = ur.IsCorrect,
                        CorrectAnswerId = correctAnswer?.Id,
                        CorrectAnswerText = correctAnswer?.Text
                    };
                }).ToList()
            };

            return View(viewModel);
        }
    }
} 