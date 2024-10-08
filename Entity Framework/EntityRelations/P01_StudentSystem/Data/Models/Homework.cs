﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomweorkId { get; set; }
        public string Content { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }


        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]

        public Course Course { get; set; }

    }

    public enum ContentType
    {
        Application,
        Pdf,
        Zip
    }
}