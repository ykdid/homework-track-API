using Homework_track_API.Entities;
using Homework_track_API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Data
{
    public class HomeworkTrackDbContext : DbContext
    {
        public HomeworkTrackDbContext(DbContextOptions<HomeworkTrackDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Homework> Homeworks { get; set; } 
        public DbSet<Student> Students { get; set; } 
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Teacher>()
                .ToTable("Teacher", schema: "Users");
            
            modelBuilder.Entity<Student>()
                .ToTable("Student", schema: "Users");
            
            modelBuilder.Entity<Homework>()
                .ToTable("Homework", schema: "Operations");
            
            modelBuilder.Entity<Submission>()
                .ToTable("Submission", schema: "Operations");
            
            
            modelBuilder.Entity<Teacher>()
                .HasKey(au => au.Id);

            modelBuilder.Entity<Student>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Homework>()
                .HasKey(ja => ja.Id);

            modelBuilder.Entity<Submission>()
                .HasKey(ae => ae.Id);
            
            modelBuilder.Entity<Homework>()
                .Property(ja => ja.Description)
                .HasColumnType("TEXT");
            
            modelBuilder.Entity<Homework>()
                .Property(a => a.Status)
                .HasColumnType("integer") 
                .HasConversion(
                    v => (int)v, 
                    v => (HomeworkStatus)v); 
        }
    }
}