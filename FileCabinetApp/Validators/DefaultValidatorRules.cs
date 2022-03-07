namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default rules for validating.
    /// </summary>
    public static class DefaultValidatorRules
    {
        /// <summary>
        /// Minimal valid first name length.
        /// </summary>
        public const int FirstNameMinLen = 2;

        /// <summary>
        /// Maximun valid last name length.
        /// </summary>
        public const int FirstNameMaxLen = 60;

        /// <summary>
        /// Minimum valid last name length.
        /// </summary>
        public const int LastNameMinLen = 2;

        /// <summary>
        /// Maximum valid last name length.
        /// </summary>
        public const int LastNameMaxLen = 60;

        /// <summary>
        /// Minimum valid school grade value.
        /// </summary>
        public const short SchoolGradeMinValue = 1;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        public const short SchoolGradeMaxValue = 11;

        /// <summary>
        /// Minimum valid average mark value.
        /// </summary>
        public const decimal AverageMarkMinValue = 0m;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        public const decimal AverageMarkMaxValue = 10m;

        /// <summary>
        /// Minimum valid class letter value.
        /// </summary>
        public const char ClassLetterMinValue = 'A';

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        public const char ClassLetterMaxValue = 'E';

        /// <summary>
        /// Gets or sets minimum valid date of birth value.
        /// </summary>
        /// <value>Minimum valid date of birth value.</value>
        public static DateTime DateOfBirthMinValue { get; set; } = new (1950, 1, 1);

        /// <summary>
        /// Gets maximum valid date of birth value.
        /// </summary>
        /// <value>Maximum valid date of birth value.</value>
        public static DateTime DateOfBirthMaxValue { get => DateTime.Now; }
    }
}
