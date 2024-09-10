using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; } 
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .IsRequired(false);

            modelBuilder.Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Resource>()
                .Property(r => r.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(s => s.Birthday)
                .IsRequired(false);

            modelBuilder.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured == false)
        //    {
        //        string connectionString = "Server=.;Database=StudentSystem;Integrated Security=True;";

        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}
    }
}
