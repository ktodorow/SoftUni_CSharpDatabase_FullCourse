using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [MaxLength(80)]
        [Unicode]
        public string Name { get; set; } = null!;
        [Unicode]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }
    }
}
