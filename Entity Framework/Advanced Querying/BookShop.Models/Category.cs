using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Category
    {
        public Category()
        {
            CategoryBooks = new HashSet<BookCategory>();   
        }

        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        [Unicode]
        public string Name { get; set; }
        
        public virtual ICollection<BookCategory> CategoryBooks { get; set; }
    }
}
