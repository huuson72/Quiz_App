using System;
using System.Linq;
using Quiz_App.Models;

namespace Quiz_App.Data
{
    public static class DbInitializer
    {
        public static void Initialize(QuizDbContext context)
        {
            // Make sure the database is created
            context.Database.EnsureCreated();

            // Check if there are already quizzes
            if (context.Quizzes.Any())
            {
                return; // DB has already been seeded
            }

            // Create a sample quiz
            var quiz = new Quiz
            {
                Title = "Basic Programming Knowledge",
                Description = "Test your knowledge of programming fundamentals",
                PassingScore = 70
            };

            context.Quizzes.Add(quiz);
            context.SaveChanges();

            // Create questions
            var questions = new Question[]
            {
                new Question
                {
                    Text = "What does HTML stand for?",
                    QuizId = quiz.Id
                },
                new Question
                {
                    Text = "Which programming language is known for its use in data science and machine learning?",
                    QuizId = quiz.Id
                },
                new Question
                {
                    Text = "What does CSS stand for?",
                    QuizId = quiz.Id
                },
                new Question
                {
                    Text = "Which of the following is a JavaScript framework?",
                    QuizId = quiz.Id
                },
                new Question
                {
                    Text = "Which data structure uses LIFO (Last In, First Out) principle?",
                    QuizId = quiz.Id
                }
            };

            context.Questions.AddRange(questions);
            context.SaveChanges();

            // Create answers for question 1
            var q1Answers = new Answer[]
            {
                new Answer
                {
                    Text = "Hyper Text Markup Language",
                    IsCorrect = true,
                    QuestionId = questions[0].Id
                },
                new Answer
                {
                    Text = "High Technical Modern Language",
                    IsCorrect = false,
                    QuestionId = questions[0].Id
                },
                new Answer
                {
                    Text = "Hyperlinks and Text Markup Language",
                    IsCorrect = false,
                    QuestionId = questions[0].Id
                },
                new Answer
                {
                    Text = "Home Tool Markup Language",
                    IsCorrect = false,
                    QuestionId = questions[0].Id
                }
            };

            // Create answers for question 2
            var q2Answers = new Answer[]
            {
                new Answer
                {
                    Text = "Java",
                    IsCorrect = false,
                    QuestionId = questions[1].Id
                },
                new Answer
                {
                    Text = "C#",
                    IsCorrect = false,
                    QuestionId = questions[1].Id
                },
                new Answer
                {
                    Text = "Python",
                    IsCorrect = true,
                    QuestionId = questions[1].Id
                },
                new Answer
                {
                    Text = "PHP",
                    IsCorrect = false,
                    QuestionId = questions[1].Id
                }
            };

            // Create answers for question 3
            var q3Answers = new Answer[]
            {
                new Answer
                {
                    Text = "Computer Style Sheets",
                    IsCorrect = false,
                    QuestionId = questions[2].Id
                },
                new Answer
                {
                    Text = "Creative Style Sheets",
                    IsCorrect = false,
                    QuestionId = questions[2].Id
                },
                new Answer
                {
                    Text = "Cascading Style Sheets",
                    IsCorrect = true,
                    QuestionId = questions[2].Id
                },
                new Answer
                {
                    Text = "Colorful Style Sheets",
                    IsCorrect = false,
                    QuestionId = questions[2].Id
                }
            };

            // Create answers for question 4
            var q4Answers = new Answer[]
            {
                new Answer
                {
                    Text = "Java",
                    IsCorrect = false,
                    QuestionId = questions[3].Id
                },
                new Answer
                {
                    Text = "React",
                    IsCorrect = true,
                    QuestionId = questions[3].Id
                },
                new Answer
                {
                    Text = "SQL",
                    IsCorrect = false,
                    QuestionId = questions[3].Id
                },
                new Answer
                {
                    Text = "Python",
                    IsCorrect = false,
                    QuestionId = questions[3].Id
                }
            };

            // Create answers for question 5
            var q5Answers = new Answer[]
            {
                new Answer
                {
                    Text = "Queue",
                    IsCorrect = false,
                    QuestionId = questions[4].Id
                },
                new Answer
                {
                    Text = "Stack",
                    IsCorrect = true,
                    QuestionId = questions[4].Id
                },
                new Answer
                {
                    Text = "Linked List",
                    IsCorrect = false,
                    QuestionId = questions[4].Id
                },
                new Answer
                {
                    Text = "Array",
                    IsCorrect = false,
                    QuestionId = questions[4].Id
                }
            };

            context.Answers.AddRange(q1Answers);
            context.Answers.AddRange(q2Answers);
            context.Answers.AddRange(q3Answers);
            context.Answers.AddRange(q4Answers);
            context.Answers.AddRange(q5Answers);
            context.SaveChanges();
        }
    }
}