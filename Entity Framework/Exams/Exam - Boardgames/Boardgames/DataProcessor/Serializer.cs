namespace Boardgames.DataProcessor
{
    using Newtonsoft.Json;
    using System.Linq;

    using Data;
    using Boardgames.Utilities;
    using DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            const string rootElement = "Creators";

            var creatorsToExport = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDto()
                {
                    CreatorName = string.Join(" ", c.FirstName, c.LastName),
                    BoardgamesCount = c.Boardgames.Count,
                    Boardgames = c.Boardgames
                        .Select(b => new ExportCreatorBoardgamesDto()
                        {
                            BoardgameName = b.Name,
                            BoardgameYearPublished = b.YearPublished,
                        })
                        .OrderBy(b => b.BoardgameName)
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            return XmlHelper.Serialize(creatorsToExport, rootElement).ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellersToExport = context.Sellers
                    .Where(s => s.BoardgamesSellers.Any())
                    .Select(s => new
                    {
                        Name = s.Name,
                        Website = s.Website,
                        Boardgames = s.BoardgamesSellers
                            .Where(bs => bs.Boardgame.YearPublished >= year)
                            .Where(bs => bs.Boardgame.Rating <= rating)
                            .Select(bs => new ExportSellerBoardgameDto()
                            {
                                Name = bs.Boardgame.Name,
                                Rating = bs.Boardgame.Rating,
                                Mechanics = bs.Boardgame.Mechanics,
                                Category = bs.Boardgame.CategoryType.ToString()
                            })
                            .OrderByDescending(bs => bs.Rating)
                            .ThenBy(bs => bs.Name)
                            .ToList()
                    })
                    .OrderByDescending(s => s.Boardgames.Count())
                    .ThenBy(s => s.Name)
                    .Take(5)
                    .ToList();

            return JsonConvert.SerializeObject(sellersToExport, Formatting.Indented);
        }
    }
}