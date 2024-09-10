namespace TravelAgency.DataProcessor.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("Guide")]
    public class ExportGuideDto
    {
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;
        
        [XmlArray("TourPackages")]
        public ExportTourPackageDto[] TourPackages { get; set; } = null!;
    }
}
