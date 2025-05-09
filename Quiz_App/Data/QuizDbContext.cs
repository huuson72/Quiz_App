using Microsoft.EntityFrameworkCore;
using Quiz_App.Models;

namespace Quiz_App.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
        public DbSet<UserResponse> UserResponses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizAttempt>()
                .HasOne(qa => qa.Quiz)
                .WithMany(q => q.Attempts)
                .HasForeignKey(qa => qa.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserResponse>()
                .HasOne(ur => ur.QuizAttempt)
                .WithMany(qa => qa.UserResponses)
                .HasForeignKey(ur => ur.QuizAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserResponse>()
                .HasOne(ur => ur.Question)
                .WithMany(q => q.UserResponses)
                .HasForeignKey(ur => ur.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserResponse>()
                .HasOne(ur => ur.Answer)
                .WithMany()
                .HasForeignKey(ur => ur.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
} 