using Microsoft.AspNetCore.Mvc;
using Quiz_App.Data;
using Quiz_App.Models;
using System.Diagnostics;
using Quiz_App.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Quiz_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuizDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(QuizDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quizzes
                .Select(q => new QuizListItemViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    QuestionCount = q.Questions.Count
                })
                .ToListAsync();

            return View(quizzes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 