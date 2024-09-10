namespace TravelAgency.DataProcessor.ExportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("TourPackage")]
    public class ExportTourPackageDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;
        
        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Price")]
        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        public decimal Price { get; set; }
    }
}
