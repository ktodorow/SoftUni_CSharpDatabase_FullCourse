namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            const string rootElement = "Clients";

            StringBuilder sb = new();
            ImportClientDto[] deserializedClients = XmlHelper.Deserialize<ImportClientDto[]>(xmlString, rootElement);

            ICollection<Client> clientsToImport = new List<Client>();

            foreach (ImportClientDto dto in deserializedClients)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> addressesToImport = new List<Address>();

                foreach (ImportAddressDto address in dto.Addresses)
                {
                    if (!IsValid(address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address newAddress = new Address()
                    {
                        StreetName = address.StreetName,
                        StreetNumber = address.StreetNumber,
                        PostCode = address.PostCode,
                        City = address.City,
                        Country = address.Country,
                    };

                    addressesToImport.Add(newAddress);
                }

                Client newClient = new Client()
                {
                    Name = dto.Name,
                    NumberVat = dto.NumberVat,
                    Addresses = addressesToImport,
                };

                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, dto.Name));
            }

            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();
            ImportInvoiceDto[] deserializedInvoices = JsonConvert
                .DeserializeObject<ImportInvoiceDto[]>(jsonString);

            ICollection<Invoice> invoicesToImport = new List<Invoice>();

            foreach (ImportInvoiceDto dto in deserializedInvoices)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isIssueDateValid = DateTime
                    .TryParse(dto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate);
                bool isDueDateValid = DateTime
                    .TryParse(dto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                if (isDueDateValid == false || isIssueDateValid == false || 
                        DateTime.Compare(dueDate, issueDate) < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Clients.Any(c => c.Id == dto.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice newInvoice = new Invoice()
                {
                    Number = dto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = dto.Amount,
                    CurrencyType = (CurrencyType)dto.CurrencyType,
                    ClientId = dto.ClientId,
                };

                invoicesToImport.Add(newInvoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, dto.Number));
            }

            context.Invoices.AddRange(invoicesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();

            ImportProductDto[] deserializedProducts = JsonConvert
                .DeserializeObject<ImportProductDto[]>(jsonString);

            ICollection<Product> productsToImport = new List<Product>();

            foreach (ImportProductDto productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product newProduct = new Product()
                {
                    Name= productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType,
                };

                ICollection<ProductClient> productClientsToImport = new List<ProductClient>();

                foreach (int clientId in productDto.Clients.Distinct())
                {
                    if (!context.Clients.Any(cl => cl.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient()
                    {
                        Product = newProduct,
                        ClientId = clientId,
                    };

                    productClientsToImport.Add(productClient);
                }

                newProduct.ProductsClients = productClientsToImport;

                productsToImport.Add(newProduct);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, productDto.Name, productClientsToImport.Count));
            }

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
