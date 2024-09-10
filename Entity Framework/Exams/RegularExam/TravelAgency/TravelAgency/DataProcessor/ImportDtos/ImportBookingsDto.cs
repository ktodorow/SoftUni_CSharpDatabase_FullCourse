
namespace TravelAgency.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstraints;
    public class ImportBookingsDto
    {
        [Required]
        public string BookingDate { get; set; } = null!;

        [Required]
        [MinLength(CustomerFullNameMinLength)]
        [MaxLength(CustomerFullNameMaxLength)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [MinLength(TourPackagePackageNameMinLength)]
        [MaxLength(TourPackagePackageNameMaxLength)]
        public string TourPackageName { get; set; } = null!;
    }
}
