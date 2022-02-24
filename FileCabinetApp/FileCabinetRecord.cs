using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents the record with personal information.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Minimal valid first name length.
        /// </summary>
        protected internal const int FirstNameMinLen = 2;

        /// <summary>
        /// Maximun valid last name length.
        /// </summary>
        protected internal const int FirstNameMaxLen = 60;

        /// <summary>
        /// Minimum valid last name length.
        /// </summary>
        protected internal const int LastNameMinLen = 2;

        /// <summary>
        /// Maximum valid last name length.
        /// </summary>
        protected internal const int LastNameMaxLen = 60;

        /// <summary>
        /// Minimum valid school grade value.
        /// </summary>
        protected internal const short SchoolGradeMinValue = 1;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        protected internal const short SchoolGradeMaxValue = 11;

        /// <summary>
        /// Minimum valid average mark value.
        /// </summary>
        protected internal const decimal AverageMarkMinValue = 0m;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        protected internal const decimal AverageMarkMaxValue = 10m;

        /// <summary>
        /// Minimum valid class letter value.
        /// </summary>
        protected internal const char ClassLetterMinValue = 'A';

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        protected internal const char ClassLetterMaxValue = 'E';

        /// <summary>
        /// Minimum valid date of birth value.
        /// </summary>
        protected internal static readonly DateTime DateOfBirthMinValue = new (1950, 1, 1);

        /// <summary>
        /// Maximum valid date of birth value.
        /// </summary>
        protected internal static readonly DateTime DateOfBirthMaxValue = DateTime.Now;

        /// <summary>
        /// Gets or sets <see cref="DateTime">DateTime</see> format for record input.
        /// </summary>
        /// <value><see cref="DateTime">DateTime</see> format for record input.</value>
        public static string InputDateTimeFormat { get; protected set; } = "MM/dd/yyyy";

        /// <summary>
        /// Gets or sets <see cref="DateTime">DateTime</see> format for record output.
        /// </summary>
        /// <value><see cref="DateTime">DateTime</see> format for record output.</value>
        public static string OutputDateTimeFormat { get; protected set; } = "yyyy-MMM-dd";

        /// <summary>
        /// Gets or sets id of record.
        /// </summary>
        /// <value>Id of record.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of a person in record.
        /// </summary>
        /// <value>First name of a person in record.</value>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets last name of a person in record.
        /// </summary>
        /// <value>Last name of a person in record.</value>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth of a person in record.
        /// </summary>
        /// <value>Date of birth of a person in record.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets school grade of a person in record.
        /// </summary>
        /// <value>School grade of a person in record.</value>
        public short SchoolGrade { get; set; }

        /// <summary>
        /// Gets or sets averange mark of a person in record.
        /// </summary>
        /// <value>Average mark of a person in record.</value>
        public decimal AverageMark { get; set; }

        /// <summary>
        /// Gets or sets class letter of a person in record.
        /// </summary>
        /// <value>Class letter of a person in record.</value>
        public char ClassLetter { get; set; }

        /// <summary>
        /// Check first name for valid value.
        /// </summary>
        /// <param name="firstName">Value to check.</param>
        /// <exception cref="ArgumentNullException">If firstName param is null.</exception>
        /// <exception cref="ArgumentException">If firstName has invalid length or contains invalid symbols.</exception>
        public static void ValidateFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (firstName.Length < FileCabinetRecord.FirstNameMinLen ||
                firstName.Length > FileCabinetRecord.FirstNameMaxLen ||
                !firstName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(firstName)} must contatin " +
                    $"from {FileCabinetRecord.FirstNameMinLen} " +
                    $"to {FileCabinetRecord.FirstNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="lastName">Value to check.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="lastName"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="lastName"/> has invalid length or contains invalid symbols.</exception>
        public static void ValidateLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (lastName.Length < FileCabinetRecord.LastNameMinLen ||
                lastName.Length > FileCabinetRecord.LastNameMaxLen ||
                !lastName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(lastName)} must contatin " +
                    $"from {FileCabinetRecord.LastNameMinLen} " +
                    $"to {FileCabinetRecord.LastNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="dateOfBirth">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="dateOfBirth"/> is less than <see cref="DateOfBirthMinValue"/> or more than <see cref="DateOfBirthMaxValue"/>.</exception>
        public static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < FileCabinetRecord.DateOfBirthMinValue ||
                dateOfBirth > FileCabinetRecord.DateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
            }
        }

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="schoolGrade">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="schoolGrade"/> is less than <see cref="SchoolGradeMinValue"/> or more than <see cref="SchoolGradeMaxValue"/>.</exception>
        public static void ValidateSchoolGrade(short schoolGrade)
        {
            if (schoolGrade < FileCabinetRecord.SchoolGradeMinValue ||
                schoolGrade > FileCabinetRecord.SchoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(schoolGrade));
            }
        }

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="averageMark">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="averageMark"/> is less than <see cref="AverageMarkMinValue"/> or more than <see cref="AverageMarkMaxValue"/>.</exception>
        public static void ValidateAverageMark(decimal averageMark)
        {
            if (averageMark < FileCabinetRecord.AverageMarkMinValue ||
                averageMark > FileCabinetRecord.AverageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(averageMark));
            }
        }

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="classLetter">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="classLetter"/> is less than <see cref="ClassLetterMinValue"/> or more than <see cref="ClassLetterMaxValue"/>.</exception>
        public static void ValidateClassLetter(char classLetter)
        {
            classLetter = char.ToUpperInvariant(classLetter);
            if (classLetter < FileCabinetRecord.ClassLetterMinValue ||
                classLetter > FileCabinetRecord.ClassLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(classLetter));
            }
        }

        /// <summary>
        /// Convert instance to its string representation.
        /// </summary>
        /// <returns>Instance string representation.</returns>
        public override string ToString()
        {
            return
                $"#{this.Id}, {this.FirstName}, {this.LastName}, " +
                $"{this.DateOfBirth.ToString(FileCabinetRecord.OutputDateTimeFormat, CultureInfo.InvariantCulture)}, " +
                $"{this.SchoolGrade}, {this.AverageMark}, {this.ClassLetter}";
        }
    }
}
