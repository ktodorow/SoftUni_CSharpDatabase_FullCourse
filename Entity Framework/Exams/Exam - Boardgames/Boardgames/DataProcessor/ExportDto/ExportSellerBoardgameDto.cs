namespace Boardgames.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstraints;

    public class ExportSellerBoardgameDto
    {
        [Required]
        [MinLength(BoardgameNameMinLength)]
        [MaxLength(BoardgameNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(BoardgameRatingMinValue, BoardgameRatingMaxValue)]
        public double Rating { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;
    }
}
