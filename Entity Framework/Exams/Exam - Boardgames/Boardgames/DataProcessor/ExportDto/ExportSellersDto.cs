namespace Boardgames.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstraints;

    public class ExportSellersDto
    {
        [Required]
        [MinLength(SellerNameMinLength)]
        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^www\.[A-Za-z0-9-]+\.com$")]
        public string Website { get; set; } = null!;

        [Required]
        public ExportSellerBoardgameDto[] Boardgames { get; set; } = null!;
    }
}