using Jeopardy.Models;
using Microsoft.EntityFrameworkCore;

namespace Jeopardy.Data {    
    public class JeopardyContext : DbContext {
        public JeopardyContext(DbContextOptions<JeopardyContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<Game>().ToTable("Games");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Answer>().ToTable("Answers");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}