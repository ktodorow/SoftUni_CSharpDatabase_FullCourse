using Microsoft.EntityFrameworkCore.Proxies.Internal;
using TravelAgency.Data.Models;

namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportCustomerDto
    {
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public ExportBookingDto[] Bookings { get; set; } = null!;
    }
}
