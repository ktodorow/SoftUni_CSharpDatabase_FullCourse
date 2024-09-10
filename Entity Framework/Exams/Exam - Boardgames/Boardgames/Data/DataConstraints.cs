namespace Boardgames.Data
{
    using Models.Enums;
    public static class DataConstraints
    {
        public const byte BoardgameNameMinLength = 10;
        public const byte BoardgameNameMaxLength = 20;
        public const double BoardgameRatingMinValue = 1;
        public const double BoardgameRatingMaxValue = 10.00;
        public const int BoardgameYearPublishedMinValue = 2018;
        public const int BoardgameYearPublishedMaxValue = 2023;
        public const int BoardgameCategoryTypeMinValue = (int)CategoryType.Abstract;
        public const int BoardgameCategoryTypeMaxValue = (int)CategoryType.Strategy;

        public const byte SellerNameMinLength = 5;
        public const byte SellerNameMaxLength = 20;
        public const byte SellerAddressMinLength = 2;
        public const byte SellerAddressMaxLength = 30;

        public const byte CreatorFirstNameMinLength = 2; 
        public const byte CreatorFirstNameMaxLength = 7;
        public const byte CreatorLastNameMinLength = 2;
        public const byte CreatorLastNameMaxLength = 7;
    }
}
