using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [MaxLength(50)]
        [Unicode]
        [Required]
        public string Name { get; set; } = null!;

        public string Url { get; set; } = null!;
        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }


    }

    public enum ResourceType
    {
        Video,
        Presentation,
        Document,
        Other
    }
}
