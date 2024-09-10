using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    public class BookCategory
    {
        [Required]
        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]

        public Category Category { get; set; } 
    }
}
