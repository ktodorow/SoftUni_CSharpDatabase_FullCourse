namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstraints;

    public class Seller
    {
        public Seller()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SellerAddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        // LOOKAGAIN: website check constraint
        /* First four characters are "www.", followed by upper and lower letters, 
           digits or '-' and the last three characters are ".com". */
        public string Website { get; set; } = null!;
     
        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}
