namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            
            //DbInitializer.ResetDatabase(context);

            Console.WriteLine(GetGoldenBooks(context));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return string.Empty;
            }

            var bookTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookTitles = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, bookTitles);
        }
        //public static string GetBooksByPrice(BookShopContext context)
        //{
        //    var bookTitles = context.Books
        //        .Where(b => b.Price > 40)
        //        .Select(b => new Book
        //        {
        //            Title = b.Title,
        //            BookId = b.BookId
        //        })
        //        .OrderBy(b => b)
        //        .ToList();

        //}
    }
}


