namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstraints;

    public class TourPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TourPackagePackageNameMaxLength)]
        public string PackageName { get; set; } = null!;

        [MaxLength(TourPackageDescriptionMaxLength)]
        public string Description { get; set; }

        // LOOKAGAIN: Not a negative number.
        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new HashSet<TourPackageGuide>();
    }
}
