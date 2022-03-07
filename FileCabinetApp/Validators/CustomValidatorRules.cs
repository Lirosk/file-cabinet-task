namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom rules for validating.
    /// </summary>
    public static class CustomValidatorRules
    {
        /// <summary>
        /// Gets or sets minimum length for name.
        /// </summary>
        /// <value>Minimum length for name.</value>
        public const int NameMinLen = 2;

        /// <summary>
        /// Gets or sets maximum length for name.
        /// </summary>
        /// <value>Maximum length for name.</value>
        public const int NameMaxLen = 10;

        /// <summary>
        /// Minimum valid school grade value.
        /// </summary>
        public const short SchoolGradeMinValue = 0;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        public const short SchoolGradeMaxValue = 4;

        /// <summary>
        /// Minimum valid average mark value.
        /// </summary>
        public const decimal AverageMarkMinValue = 0m;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        public const decimal AverageMarkMaxValue = 5m;

        /// <summary>
        /// Minimum valid class letter value.
        /// </summary>
        public const char ClassLetterMinValue = 'A';

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        public const char ClassLetterMaxValue = 'G';

        /// <summary>
        /// Gets or sets minimum valid date of birth value.
        /// </summary>
        /// <value>Minimum valid date of birth value.</value>
        public static readonly DateTime DateOfBirthMinValue = new (1950, 1, 1);

        /// <summary>
        /// Gets or sets maximum valid date of birth value.
        /// </summary>
        /// <value>Maximum valid date of birth value.</value>
        public static readonly DateTime DateOfBirthMaxValue = new (2016, 1, 1);
    }
}
