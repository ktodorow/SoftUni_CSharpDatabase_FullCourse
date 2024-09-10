namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class TourPackageGuide
    {
        [Required]
        [ForeignKey(nameof(TourPackage))]
        public int TourPackageId { get; set; }
        public virtual TourPackage TourPackage { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Guide))]
        public int GuideId { get; set; }
        public virtual Guide Guide { get; set; } = null!;
    }
}
