using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        public int AuthorId { get; set; }

        [MaxLength(50)]
        [Unicode]
        public string? FirstName { get; set; }

        [Required]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
