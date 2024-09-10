namespace TravelAgency.DataProcessor
{
    using Newtonsoft.Json;
    using System.Globalization;
    using TravelAgency.Data;
    using TravelAgency.Data.Models;
    using TravelAgency.Data.Models.Enums;
    using TravelAgency.DataProcessor.ExportDtos;
    using TravelAgency.Utilities;

    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var guidesToExport = context.Guides
                .Where(g => g.Language == (Language)3)//spanish
                .Select(g => new ExportGuideDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                        .Select (g => new ExportTourPackageDto()
                        {
                            Name = g.TourPackage.PackageName,
                            Description = g.TourPackage.Description,
                            Price = g.TourPackage.Price,
                        })
                        .OrderByDescending(tp => tp.Price)
                        .ThenBy(tp => tp.Name)
                        .ToArray()
                })
                .OrderByDescending(g => g.TourPackages.Count())
                .ThenBy(g => g.FullName)
                .ToArray();

            return XmlHelper.Serialize(guidesToExport, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new
                {
                    c.FullName,
                    c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .Select(b => new
                        {
                            b.TourPackage.PackageName,
                            b.BookingDate
                        })
                        .ToList()
                })
                .OrderByDescending(c => c.Bookings.Count())
                .ThenBy(c => c.FullName)
                .ToList();

            var customersToExport = customers
                .Select(c => new ExportCustomerDto()
                {
                    FullName= c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Select(b => new ExportBookingDto()
                        {
                            TourPackageName = b.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        })
                        .OrderBy(b => b.Date)
                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(customersToExport, Formatting.Indented);

        }
    }
}
