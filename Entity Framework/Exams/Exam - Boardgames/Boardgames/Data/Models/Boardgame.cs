
namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Models.Enums;
    using static DataConstraints;

    public class Boardgame
    {
        public Boardgame()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(BoardgameNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        // LOOKAGAIN: Range is between 1 and 10.00
        public double Rating { get; set; }

        [MaxLength(BoardgameYearPublishedMaxValue)]
        public int YearPublished { get; set; }

        // LOOKAGAIN: Range is between "Abstract" and "Strategy"
        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        public virtual Creator Creator { get; set; } = null!;
        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}
