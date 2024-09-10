namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstraints;
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(CustomerEmailMaxLength)]
        public string Email { get; set; } = null!;

        // LOOKAGAIN: All phone numbers must have the following structure: a plus sign followed by 12 digits, without spaces or special characters.
        // use [RegularExpression]
        [Required]
        [MaxLength(CustomerPhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set;} = new HashSet<Booking>();
    }
}
