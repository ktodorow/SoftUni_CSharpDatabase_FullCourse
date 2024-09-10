namespace TravelAgency.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using TravelAgency.Data;
    using TravelAgency.Utilities;
    using TravelAgency.DataProcessor.ImportDtos;
    using TravelAgency.Data.Models;
    using Newtonsoft.Json;
    using System.Globalization;
    using Microsoft.VisualBasic;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        // DONE
        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new();

            const string xmlRoot = "Customers";

            ImportCustomerDto[] deserializedCustomers = XmlHelper
                        .Deserialize<ImportCustomerDto[]>(xmlString, xmlRoot);

            ICollection<Customer> customersToImport = new List<Customer>();

            foreach (ImportCustomerDto customerDto in deserializedCustomers)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (customersToImport.Any(c => c.FullName == customerDto.FullName) ||
                    customersToImport.Any(c => c.Email == customerDto.Email) ||
                    customersToImport.Any(c => c.PhoneNumber == customerDto.PhoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer newCustomer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                };

                customersToImport.Add(newCustomer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customerDto.FullName));
            }

            context.Customers.AddRange(customersToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new();

            ImportBookingsDto[] deserializedBooks = JsonConvert
                    .DeserializeObject<ImportBookingsDto[]>(jsonString)!;

            ICollection<Booking> bookingsToImport = new List<Booking>();

            foreach (ImportBookingsDto bookingDto in deserializedBooks)
            {
                bool isBookingDateValid = DateTime
                        .TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);

                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (isBookingDateValid == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Customer customer = context.Customers.FirstOrDefault(c => c.FullName == bookingDto.CustomerName)!;
                TourPackage tourPackage = context.TourPackages.FirstOrDefault(tp => tp.PackageName == bookingDto.TourPackageName)!;

                if (customer == null || tourPackage == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Booking newBooking = new Booking()
                {
                    BookingDate = bookingDate,
                    Customer = customer,
                    TourPackage = tourPackage,
                };

                bookingsToImport.Add(newBooking);
                sb.AppendLine(string.Format
                    (SuccessfullyImportedBooking, bookingDto.TourPackageName, bookingDate.ToString("yyyy-MM-dd")));
            }

            context.Bookings.AddRange(bookingsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
