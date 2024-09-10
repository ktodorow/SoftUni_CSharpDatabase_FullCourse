using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            //string path = "../../../Datasets/users.xml";
            //Console.WriteLine(ImportUsers(context, path));

            //string path = "../../../Datasets/products.xml";
            //Console.WriteLine(ImportProducts(context, path));

            //string path = "../../../Datasets/categories.xml";
            //Console.WriteLine(ImportCategories(context, path));

            //string path = "../../../Datasets/categories-products.xml";
            //Console.WriteLine(ImportCategoryProducts(context, path));

            Console.WriteLine(GetUsersWithProducts(context));
        }



        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new(typeof(List<UserImportDTO>), new XmlRootAttribute("Users"));

            List<UserImportDTO> importDTO = new();

            using (var reader = new StreamReader(inputXml))
            {
                importDTO = (List<UserImportDTO>)serializer.Deserialize(reader);
            }

            List<User> users = importDTO
                .Select(dto => new User()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age
                })
                .ToList();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductImportDTO[]), new XmlRootAttribute("Products"));

            using StringReader reader = new StringReader(inputXml);

            var importDTO = (ProductImportDTO[])serializer.Deserialize(reader);

            Product[] products = importDTO
                .Select(p => new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerId = p.BuyerId == 0 ? null : p.BuyerId,
                    SellerId = p.SellerId
                })
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesImportDTO[]), new XmlRootAttribute("Categories"));

            using StringReader reader = new StringReader(inputXml);

            var importDTO = (CategoriesImportDTO[])serializer.Deserialize(reader);

            Category[] categories = importDTO
                .Select(dto => new Category()
                {
                    Name = dto.Name
                })
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            //XmlSerializer serializer = new(typeof(CategoriesProductsDTO[]), new XmlRootAttribute("CategoryProducts"));

            //using StreamReader reader = new(inputXml); <-- STRING!!!!!!!!!!! NOT STREAM!!!!!

            //var importDTO = (CategoriesProductsDTO[])serializer.Deserialize(reader);

            XmlSerializer serializer = new(typeof(CategoriesProductsDTO[]), new XmlRootAttribute("CategoryProducts"));

            using StringReader reader = new(inputXml);

            var importDTO = (CategoriesProductsDTO[])serializer.Deserialize(reader);

            var categoryIds = context.Categories
                .Select(c => c.Id)
                .ToArray();

            var productIds = context.Products
                .Select(c => c.Id)
                .ToArray();

            var validCategoryProductIds = importDTO
                .Where(vcp => categoryIds.Contains(vcp.CategoryId) && productIds.Contains(vcp.ProductId))
                .ToArray();

            CategoryProduct[] categoryProducts = validCategoryProductIds
                .Select(cp => new CategoryProduct()
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId
                })
                .ToArray();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ProductExportDTO[] exportDTOs = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductExportDTO()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            return ToXML<ProductExportDTO[]>(exportDTOs, "Products").ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUserSoldProducts[] exportDTOs = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .Select(p => new ExportUserSoldProducts()
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    SoldProducts = p.ProductsSold
                        .Select(ps => new ExportArraySoldProductsDTO()
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            return ToXML<ExportUserSoldProducts[]>(exportDTOs, "Users").ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            CategoriesExportDTO[] exportDTOs = context.Categories
               .Select(c => new CategoriesExportDTO()
               {
                   Name = c.Name,
                   ProductsCount = c.CategoryProducts.Select(cp => cp.ProductId).Count(),
                   AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
                   TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum()
               })
               .OrderByDescending(c => c.ProductsCount)
               .ThenBy(c => c.TotalRevenue)
               .ToArray();

            return ToXML<CategoriesExportDTO[]>(exportDTOs, "Categories").ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var exportDTOs = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UsersData()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsCount()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .Select(sp => new SoldProductsData()
                            {
                                Name = sp.Name,
                                Price = sp.Price
                            })
                            .OrderByDescending(sp => sp.Price)
                            .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            UserCountExportDTO userCount = new UserCountExportDTO()
            {
                Count = context.Users.Count(u => u.ProductsSold.Count >= 1),
                Users = exportDTOs
            };

            return ToXML(userCount, "Users").ToString().TrimEnd();
        }
    }
}