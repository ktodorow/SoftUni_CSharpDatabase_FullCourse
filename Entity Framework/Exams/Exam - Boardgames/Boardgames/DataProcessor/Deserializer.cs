namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Boardgames.Utilities;
    using DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            const string rootElement = "Creators";
            StringBuilder sb = new();

            ImportCreatorDto[] deserializedCreators = XmlHelper
                    .Deserialize<ImportCreatorDto[]>(xmlString, rootElement);
        
            ICollection<Creator> creatorsToImport = new List<Creator>();

            foreach (ImportCreatorDto creatorDto in deserializedCreators)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Boardgame> boardgamesToImport = new List<Boardgame>();

                foreach (ImportBoardgameDto boardgameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame newBoardgame = new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics,
                    };

                    boardgamesToImport.Add(newBoardgame);
                }

                Creator newCreator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                    Boardgames = boardgamesToImport
                };

                creatorsToImport.Add(newCreator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator,
                            creatorDto.FirstName, creatorDto.LastName, boardgamesToImport.Count));
            }


            context.Creators.AddRange(creatorsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        //CORRECT
        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new();

            ImportSellerDto[] deserializedSellers = JsonConvert
                    .DeserializeObject<ImportSellerDto[]>(jsonString)!;

            ICollection<Seller> sellersToImport = new List<Seller>();

            foreach (ImportSellerDto sellerDto in deserializedSellers)
            {
                // TODO: website validation
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);

                    Console.WriteLine(sb.ToString());
                    continue;
                }

                Seller newSeller = new Seller() 
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                };

                ICollection<BoardgameSeller> boardgameSellersToImport = new List<BoardgameSeller>();

                foreach (int boardgameId in sellerDto.Boardgames.Distinct())
                {
                    if (!context.Boardgames.Any(b => b.Id == boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Seller = newSeller,
                        BoardgameId = boardgameId,
                    };

                    boardgameSellersToImport.Add(boardgameSeller);
                }

                newSeller.BoardgamesSellers = boardgameSellersToImport;

                sellersToImport.Add(newSeller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, sellerDto.Name, boardgameSellersToImport.Count));
            }

            context.Sellers.AddRange(sellersToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
