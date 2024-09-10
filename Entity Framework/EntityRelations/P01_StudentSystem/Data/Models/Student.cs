﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem.Data.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [MaxLength(100)]
        [Unicode]
        public string Name { get; set; } = null!;

        [MinLength(10)]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }
    }
}
