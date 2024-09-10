namespace TravelAgency.Data
{
    using TravelAgency.Data.Models.Enums;

    public static class DataConstraints
    {
        public const byte GuideFullNameMinLength = 4;
        public const byte GuideFullNameMaxLength = 60;
        public const int GuideLanguageMinValue = (int)Language.English;
        public const int GuideLanguageMaxValue = (int)Language.Russian;

        public const byte CustomerFullNameMinLength = 4;
        public const byte CustomerFullNameMaxLength = 60;
        public const byte CustomerEmailMinLength = 6;
        public const byte CustomerEmailMaxLength = 50;
        public const byte CustomerPhoneNumberMaxLength = 13;

        public const byte TourPackagePackageNameMinLength = 2;
        public const byte TourPackagePackageNameMaxLength = 40;
        public const byte TourPackageDescriptionMaxLength = 200;
    }
}
