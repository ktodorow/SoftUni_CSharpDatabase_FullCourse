namespace TravelAgency.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Data.DataConstraints;

    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [XmlElement("FullName")]
        [Required]
        [MinLength(CustomerFullNameMinLength)]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; } = null!;
        
        [XmlElement("Email")]
        [Required]
        [MinLength(CustomerEmailMinLength)]
        [MaxLength(CustomerEmailMaxLength)]
        public string Email { get; set; } = null!;

        // LOOKAGAIN: POSSIBLE REGEX IMPORT BUG
        [XmlAttribute("phoneNumber")]
        [Required]
        [MinLength(CustomerPhoneNumberMaxLength)]
        [MaxLength(CustomerPhoneNumberMaxLength)]
        [RegularExpression(@"\+\d{12}")]
        public string PhoneNumber { get; set; } = null!;
    }
}
