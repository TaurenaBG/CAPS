namespace CAPS.Global
{
    public static class GlobalConstants
    {
        public const string IsRequiredMsg = "This field is required";


        public const int PawnShopNameMaxLenght = 60;
        public const int PawnShopNameMinLenght = 2;
        public const string PawnShopNameMaxLenghtErrorMsg = "Name must be less than 60 characters";
        public const string PawnShopNameMinLenghtErrorMsg = "Name must be more than 2 characters";
               
        public const int PawnShopCityMaxLenght = 60;
        public const int PawnShopCityMinLenght = 2;
        public const string PawnShopCityMaxLenghtErrorMsg = "City must be less than 60 characters";
        public const string PawnShopCityMinLenghtErrorMsg = "City must be more than 2 characters";

        public const int UserFullNameMaxLenght = 55;
        public const string UserFullNameMaxLenghtErrorMsg = "Full name must be less than 55 characters";

        public const double CurrencyMaxAmount = 5000.00;
        public const double CurrencyMinAmount = 1.00;
        public const string CurrencyNotInRange = "The amount must be between 1.00 and 5000.00";

        public const int LoanTermMinValue = 1;
        public const int LoanTermMaxValue = 12;
        public const string LoanTermNotInRangeErrorMsg = "Term must be between 1 and 12 months";

        public const int ItemNameMaxLenght = 60;
        public const int ItemNameMinLenght = 2;
        public const string ItemNameNotInRangeErrorMsg = "Item name must be between 2 and 60 characters";

        public const int ItemDescriptionMaxLenght = 500;
        public const int ItemDescriptionMinLenght = 3;
        public const string ItemDescriptionLenghtNotInRangeErrorMsg = "Description should be atleast 3 character and no more than 500";

        public const string UserPasswordConfirmationErrorMsg = "The password and confirmation password do not match.";
    }
}
