namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Data.Models;
    using static Data.DataConstraints;

    [XmlType(nameof(Boardgame))]
    public class ImportBoardgameDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(BoardgameNameMinLength)]
        [MaxLength(BoardgameNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Rating))]
        [Required]
        //[MinLength(BoardgameRatingMinValue)]
        //[MaxLength(BoardgameRatingMaxValue)]
        [Range(BoardgameRatingMinValue, BoardgameRatingMaxValue)]
        public double Rating { get; set; }

        [XmlElement(nameof(YearPublished))]
        [Range(BoardgameYearPublishedMinValue, BoardgameYearPublishedMaxValue)]
        public int YearPublished { get; set; }

        [XmlElement(nameof(CategoryType))]
        [Range(BoardgameCategoryTypeMinValue, BoardgameCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [XmlElement(nameof(Mechanics))]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
