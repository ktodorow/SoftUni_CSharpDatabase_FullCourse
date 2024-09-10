namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstraints;
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(AddressStreetNameMaxLength)]
        public string StreetName { get; set; }

        //[Required] <-- INT IS REQUIRED BY DEFAULT
        public int StreetNumber { get; set; }
            
        [Required]
        public string PostCode { get; set; }

        [Required]
        [MaxLength(AddressCityMaxLength)]
        public string City { get; set; }

        [Required]
        [MaxLength(AddressCountryMaxLength)]
        public string Country { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [Required]
        public Client Client { get; set; } = null!;
    }
}