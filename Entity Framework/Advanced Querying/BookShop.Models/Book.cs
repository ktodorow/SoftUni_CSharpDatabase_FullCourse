using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    public class Book
    {
        public Book()
        {
            BookCategories = new HashSet<BookCategory>();    
        }

        [Key]
        public int BookId { get; set; }

        [Required]
        [MaxLength(50)]
        [Unicode]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        [Unicode]
        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Required]
        public int Copies { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public EditionType EditionType { get; set; }

        [Required]
        public AgeRestriction AgeRestriction { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public virtual ICollection<BookCategory> BookCategories { get; set; }
    }
}
