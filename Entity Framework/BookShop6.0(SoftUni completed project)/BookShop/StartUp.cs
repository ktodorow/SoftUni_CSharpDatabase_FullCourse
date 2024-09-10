namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Castle.Core.Internal;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Primitives;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();

            DbInitializer.ResetDatabase(context);

            //string input = Console.ReadLine();

            Console.WriteLine(RemoveBooks(context));
        }

        //02
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
        
        //03
        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookTitles = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new Book
                {
                    BookId = b.BookId,
                    Title = b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, bookTitles.Select(b => b.Title));
        }

        //04
        public static string GetBooksByPrice(BookShopContext context)
        {
            var bookTitlesPrices = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new Book
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            return string.Join(Environment.NewLine, bookTitlesPrices.Select(b => $"{b.Title} - ${b.Price:f2}"));
        }

        //05
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var notReleasedBooks = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new Book
                {
                    Title = b.Title,
                    BookId = b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToList();
                

            return string.Join(Environment.NewLine, notReleasedBooks.Select(b => b.Title));
        }
        
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var booksByCategories = context.BooksCategories
                .Where(bc => categories.Contains(bc.Category.Name.ToLower()))
                .Select(b => b.Book.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, booksByCategories);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime releaseDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var booksRelasedBeforeDate = context.Books
                .Where(b => b.ReleaseDate < releaseDate)
                .Select(b => new Book
                {
                    Title = b.Title,
                    EditionType = b.EditionType,
                    Price = b.Price,
                    ReleaseDate = b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            return string.Join(Environment.NewLine, booksRelasedBeforeDate.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .ToList()
                .OrderBy(a => a.FullName)
                .ToList();

            return string.Join(Environment.NewLine, authors.Select(a => a.FullName));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string ignoreCaseInput = input.ToLower();
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(ignoreCaseInput))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string ignoreCaseInput = input.ToLower();

            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(ignoreCaseInput))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}",
                })
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.AuthorFullName})"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Count(b => b.Title.Length > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    BookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.BookCopies)
                .ToList();
            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName} - {a.BookCopies}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profitByCategory = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(c => 
                            c.Book.Copies * c.Book.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToList();

            return string.Join(Environment.NewLine, profitByCategory.Select(pbc => $"{pbc.Name} ${pbc.TotalProfit:f2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var booksByCategories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3)
                        .Select(cb => new
                        {
                            cb.Book.ReleaseDate,
                            cb.Book.Title
                        })
                        .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            StringBuilder sb = new();

            foreach (var category in booksByCategories)
            {
                sb.AppendLine($"--{category.Name}");

                if (category.Books.Any())
                {
                    foreach (var book in category.Books)
                    {
                        sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var b in books)
            {
                b.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(booksToDelete);

            context.SaveChanges();

            return booksToDelete.Count;
        }
    }
}