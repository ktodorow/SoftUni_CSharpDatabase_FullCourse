using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new();
            //string jsonUserTxt = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, jsonUserTxt));

            //string jsonProductsTxt = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, jsonProductsTxt));

            //string jsonCategoryTxt = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, jsonCategoryTxt));

            //string jsonCategoriesProductsTxt = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, jsonCategoriesProductsTxt));

            Console.WriteLine(GetUsersWithProducts(context));
        }
        
        //01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
        //02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
        //03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            categories.RemoveAll(n => n.Name == null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        //04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //05
        public static string GetProductsInRange(ProductShopContext context)
        {
            //Get all products in a specified price range:  500 to 1000 (inclusive). Order them by price (from lowest to highest).
            //Select only the product name, price and the full name of the seller. Export the result to JSON.

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(products, settings);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(x => x.BuyerId != null))
                .Select(u => new SellerWithProductDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(p => new SoldProductsDTO
                        {
                            Name = p.Name,
                            Price = p.Price,
                            BuyerFirstName = p.Buyer.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        })
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            var settings = new JsonSerializerSettings()
            {
               // NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(users, settings);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = $"{Math.Round(c.CategoriesProducts.Average(p => p.Product.Price), 2):f2}",
                    TotalRevenue = $"{Math.Round(c.CategoriesProducts.Sum(p => p.Product.Price), 2):f2}"
                });

            var settings = new JsonSerializerSettings()
            {
                // NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(categories, settings);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(x => x.BuyerId != null && x.Price != null))
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = u.ProductsSold
                        .Where(sp => sp.BuyerId != null && sp.Price != null)
                        .Select(sp => new
                        {
                            Name = sp.Name,
                            Price = sp.Price
                        })
                        .ToList()
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var output = new
            {
                UsersCount = users.Count,
                Users = users
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.Age,
                        SoldProducts = new
                        {
                            Count = u.SoldProducts.Count,
                            Products = u.SoldProducts
                        }
                    })
            };

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(output, settings);
        }
    }
}