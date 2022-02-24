using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        protected internal const int FirstNameMinLen = 2;
        protected internal const int FirstNameMaxLen = 60;
        protected internal const int LastNameMinLen = 2;
        protected internal const int LastNameMaxLen = 60;
        protected internal const short SchoolGradeMinValue = 1;
        protected internal const short SchoolGradeMaxValue = 11;
        protected internal const decimal AverageMarkMinValue = 0m;
        protected internal const decimal AverageMarkMaxValue = 10m;
        protected internal const char ClassLetterMinValue = 'A';
        protected internal const char ClassLetterMaxValue = 'E';
        protected internal static readonly DateTime DateOfBirthMinValue = new DateTime(1950, 1, 1);
        protected internal static readonly DateTime DateOfBirthMaxValue = DateTime.Now;

        public static string InputDateTimeFormat { get; protected set; } = "MM/dd/yyyy";

        public static string OutputDateTimeFormat { get; protected set; } = "yyyy-MMM-dd";

        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public short SchoolGrade { get; set; }

        public decimal AverageMark { get; set; }

        public char ClassLetter { get; set; }

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

        public static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < FileCabinetRecord.DateOfBirthMinValue ||
                dateOfBirth > FileCabinetRecord.DateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
            }
        }

        public static void ValidateSchoolGrade(short schoolGrade)
        {
            if (schoolGrade < FileCabinetRecord.SchoolGradeMinValue ||
                schoolGrade > FileCabinetRecord.SchoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(schoolGrade));
            }
        }

        public static void ValidateAverageMark(decimal averageMark)
        {
            if (averageMark < FileCabinetRecord.AverageMarkMinValue ||
                averageMark > FileCabinetRecord.AverageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(averageMark));
            }
        }

        public static void ValidateClassLetter(char classLetter)
        {
            classLetter = char.ToUpperInvariant(classLetter);
            if (classLetter < FileCabinetRecord.ClassLetterMinValue ||
                classLetter > FileCabinetRecord.ClassLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(classLetter));
            }
        }

        public override string ToString()
        {
            return
                $"#{this.Id}, {this.FirstName}, {this.LastName}, " +
                $"{this.DateOfBirth.ToString(FileCabinetRecord.OutputDateTimeFormat, CultureInfo.InvariantCulture)}, " +
                $"{this.SchoolGrade}, {this.AverageMark}, {this.ClassLetter}";
        }
    }
}
