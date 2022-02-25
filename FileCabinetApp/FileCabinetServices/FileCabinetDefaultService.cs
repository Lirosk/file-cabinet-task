namespace FileCabinetApp
{
    /// <summary>
    /// Service with default data validation.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Minimal valid first name length.
        /// </summary>
        protected const int FirstNameMinLen = 2;

        /// <summary>
        /// Maximun valid last name length.
        /// </summary>
        protected const int FirstNameMaxLen = 60;

        /// <summary>
        /// Minimum valid last name length.
        /// </summary>
        protected const int LastNameMinLen = 2;

        /// <summary>
        /// Maximum valid last name length.
        /// </summary>
        protected const int LastNameMaxLen = 60;

        /// <summary>
        /// Minimum valid school grade value.
        /// </summary>
        protected const short SchoolGradeMinValue = 1;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        protected const short SchoolGradeMaxValue = 11;

        /// <summary>
        /// Minimum valid average mark value.
        /// </summary>
        protected const decimal AverageMarkMinValue = 0m;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        protected const decimal AverageMarkMaxValue = 10m;

        /// <summary>
        /// Minimum valid class letter value.
        /// </summary>
        protected const char ClassLetterMinValue = 'A';

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        protected const char ClassLetterMaxValue = 'E';

        /// <summary>
        /// Gets or sets minimum valid date of birth value.
        /// </summary>
        /// <value>Minimum valid date of birth value.</value>
        protected static DateTime DateOfBirthMinValue { get; set; } = new (1950, 1, 1);

        /// <summary>
        /// Gets maximum valid date of birth value.
        /// </summary>
        /// <value>Maximum valid date of birth value.</value>
        protected static DateTime DateOfBirthMaxValue { get => DateTime.Now; }

        /// <summary>
        /// Checks for valid values in object parameter..
        /// </summary>
        /// <param name="personalData">Object parameter contains values to check for valid.</param>
        /// <exception cref="ArgumentException">One of the values is invalid.</exception>
        protected override void Validate(PersonalData personalData)
        {
            ValidateFirstName(personalData.FirstName);
            ValidateLastName(personalData.LastName);
            ValidateDateOfBirth(personalData.DateOfBirth);
            ValidateSchoolGrade(personalData.SchoolGrade);
            ValidateAverageMark(personalData.AverageMark);
            ValidateClassLetter(personalData.ClassLetter);
        }

        /// <summary>
        /// Check first name for valid value.
        /// </summary>
        /// <param name="firstName">Value to check.</param>
        /// <exception cref="ArgumentNullException">If firstName param is null.</exception>
        /// <exception cref="ArgumentException">If firstName has invalid length or contains invalid symbols.</exception>
        private static void ValidateFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (firstName.Length < FirstNameMinLen ||
                firstName.Length > FirstNameMaxLen ||
                !firstName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(firstName)} must contatin " +
                    $"from {FirstNameMinLen} " +
                    $"to {FirstNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="lastName">Value to check.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="lastName"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="lastName"/> has invalid length or contains invalid symbols.</exception>
        private static void ValidateLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (lastName.Length < LastNameMinLen ||
                lastName.Length > LastNameMaxLen ||
                !lastName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(lastName)} must contatin " +
                    $"from {LastNameMinLen} " +
                    $"to {LastNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="dateOfBirth">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="dateOfBirth"/> is less than <see cref="DateOfBirthMinValue"/> or more than <see cref="DateOfBirthMaxValue"/>.</exception>
        private static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < DateOfBirthMinValue ||
                dateOfBirth > DateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
            }
        }

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="schoolGrade">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="schoolGrade"/> is less than <see cref="SchoolGradeMinValue"/> or more than <see cref="SchoolGradeMaxValue"/>.</exception>
        private static void ValidateSchoolGrade(short schoolGrade)
        {
            if (schoolGrade < SchoolGradeMinValue ||
                schoolGrade > SchoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(schoolGrade));
            }
        }

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="averageMark">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="averageMark"/> is less than <see cref="AverageMarkMinValue"/> or more than <see cref="AverageMarkMaxValue"/>.</exception>
        private static void ValidateAverageMark(decimal averageMark)
        {
            if (averageMark < AverageMarkMinValue ||
                averageMark > AverageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(averageMark));
            }
        }

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="classLetter">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="classLetter"/> is less than <see cref="ClassLetterMinValue"/> or more than <see cref="ClassLetterMaxValue"/>.</exception>
        private static void ValidateClassLetter(char classLetter)
        {
            classLetter = char.ToUpperInvariant(classLetter);
            if (classLetter < ClassLetterMinValue ||
                classLetter > ClassLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(classLetter));
            }
        }
    }
}
