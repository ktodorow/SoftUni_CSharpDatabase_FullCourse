using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new();

            string jsonSupplierTxt = File.ReadAllText("../../../Datasets/suppliers.json");
            Console.WriteLine(ImportSuppliers(context, jsonSupplierTxt));

            //string jsonPartsTxt = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, jsonPartsTxt));


            //string jsonCarTxt = File.ReadAllText("../../../Datasets/cars.json");
            // Console.WriteLine(ImportCars(context, jsonCarTxt));

        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }
            
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validPartsId = context.Parts
                .Select(p => p.Id)
                .ToList();

            var partsWithValidSuppliersId = parts
                .Where(p => validPartsId.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(partsWithValidSuppliersId);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDTO = JsonConvert.DeserializeObject<List<ImportCarDTO>>(inputJson);

            HashSet<Car> cars = new();
            HashSet<PartCar> partsCars = new();

            foreach (var carDTO in carsDTO)
            {
                var newCar = new Car()
                {
                    Make = carDTO.Make,
                    Model = carDTO.Model,
                    TraveledDistance = carDTO.TraveledDistance,
                };

                cars.Add(newCar);

                foreach (var id in carDTO.PartsId.Distinct())
                {
                    partsCars.Add(new PartCar()
                    {
                        Car = newCar,
                        PartId = id
                    });
                }
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);
            context.SaveChanges()
                ;
            return $"Successfully imported {cars.Count}";
        }
    }
}