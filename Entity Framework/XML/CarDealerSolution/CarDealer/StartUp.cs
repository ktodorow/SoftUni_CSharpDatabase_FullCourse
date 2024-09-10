using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new();
            //string xml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, xml));

            //string xml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, xml));

            //string xml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, xml));

            //string xml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, xml));

            string xml = File.ReadAllText("../../../Datasets/sales.xml");
            Console.WriteLine(ImportSales(context, xml));
        }

        //Serializer to XML 
        private static string ToXML<T>(T obj, string rootElement, bool omitXmlDeclaration = false)
        {
            XmlRootAttribute root = new(rootElement);
            XmlSerializer serializer = new(typeof(T), root);
            StringBuilder sb = new();

            XmlWriterSettings writerSettings = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = new UTF8Encoding(false),
                OmitXmlDeclaration = omitXmlDeclaration
            };

            using (StringWriter writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, writerSettings))
            {
                XmlSerializerNamespaces namespaces = new();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(xmlWriter, obj, namespaces);
            };

            return sb.ToString().TrimEnd();
        }

        //Deserializer
        private static T DeserializeFromXML<T>(string rootElement, string inputXml)
        {
            XmlRootAttribute root = new(rootElement);
            XmlSerializer serializer = new(typeof(T), root);

            using StringReader reader = new(inputXml);

            T importDTOs = (T)serializer.Deserialize(reader);

            return importDTOs;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var importDTOs = DeserializeFromXML<ImportSuppliersDTO[]>("Suppliers", inputXml);

            Supplier[] suppliers = importDTOs
                .Select(s => new Supplier()
                {
                    Name = s.Name,
                    IsImporter = s.isImporter
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var importDTOs = DeserializeFromXML<ImportPartsDTO[]>("Parts", inputXml);

            int[] validSupplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            Part[] parts = importDTOs
                .Where(p => validSupplierIds.Contains(p.SupplierId))
                .Select(p => new Part() 
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                })
                .ToArray();
            
            context.Parts.AddRange(parts);
            context.SaveChanges();
            
            return $"Successfully imported {parts.Length}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var importDTOs = DeserializeFromXML<ImportCarsDTO[]>("Cars", inputXml);

            List<Car> cars = new();

            foreach (var dto in importDTOs)
            {
                Car car = new()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                };

                int[] carPartsId = dto.PartIds
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                List<PartCar> carParts = new();

                foreach (var id in carPartsId)
                {
                    carParts.Add(new PartCar()
                    {
                        Car = car,
                        PartId = id
                    });
                }

                car.PartsCars = carParts;
                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var importDTOs = DeserializeFromXML<ImportCustomersDTO[]>("Customers", inputXml);

            Customer[] customers = importDTOs
                .Select(p => new Customer()
                {
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    IsYoungDriver = p.IsYoungDriver
                })
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var importDTOs = DeserializeFromXML<ImportSalesDTO[]>("Sales", inputXml);

            int[] validCarIds = context.Cars.Select(c => c.Id).ToArray();
            int[] validCustomerIds = context.Customers.Select(c => c.Id).ToArray();

            Sale[] sales = importDTOs
                .Where(s => validCarIds.Contains(s.CarId) &&
                            validCustomerIds.Contains(s.CustomerId))
                .Select(s => new Sale()
                {
                    CarId = s.CarId,
                    CustomerId = s.CustomerId,
                    Discount = s.Discount
                })
                .ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var exportDtos = context.Cars
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarWithDistanceDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            return ToXML<ExportCarWithDistanceDTO[]>(exportDtos, "cars").ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var exportDtos = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportCarsFromMakeDTO()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToArray();

            return ToXML<ExportCarsFromMakeDTO[]>(exportDtos, "cars", true).ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var exportDtos = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDTO()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            return ToXML<ExportLocalSuppliersDTO[]>(exportDtos, "suppliers").ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var exportDtos = context.Cars
                .Select(c => new ExportCarsWithPartsDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars
                        .Select(pc => new PartsOfCarExportDTO()
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            return ToXML<ExportCarsWithPartsDTO[]>(exportDtos, "cars").ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var temp = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SalesData = c.Sales
                        .Select(s => new
                        {
                            Prices = c.IsYoungDriver
                            ? s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                            : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                        })
                        .ToArray()
                })
                .ToArray();

            var customerTotalSales = temp
                .OrderByDescending(x => x.SalesData.Sum(x => x.Prices))
                .Select(x => new ExportCustomerTotalSalesDTO()
                {
                    FullName = x.FullName,
                    BoughtCars = x.BoughtCars,
                    SpentMoney = x.SalesData.Sum(x => (decimal)x.Prices)
                })
                .ToArray();

            return ToXML(customerTotalSales, "customers").ToString().TrimEnd();
        }


        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var exportDtos = context.Sales
                .Select(s => new ExportSalesDiscountsDTO()
                {
                    Car = new CarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },

                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars
                        .Sum(pc => pc.Part.Price),

                    PriceWithDiscount = Math.Round(
                        (double)(s.Car.PartsCars.Sum(p => p.Part.Price)
                                            * (1 - (s.Discount / 100))), 4)
                })
                .ToArray();

            return ToXML<ExportSalesDiscountsDTO[]>(exportDtos, "sales").ToString().TrimEnd();
        }

    }
}