using System.Globalization;
using FileCabinetApp.Validators;
using Models;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Some help with working with records.
    /// </summary>
    public static class RecordHelper
    {
        /// <summary>
        /// Reads record from console.
        /// </summary>
        /// <param name="dateTimeFormat">Format for input datetime values.</param>
        /// <param name="personalData">Resulting personal data.</param>
        public static void ReadRecordDataFromConsole(string dateTimeFormat, out PersonalData personalData)
        {
            personalData = new ();

            Console.Write("First name: ");
            personalData.FirstName =
                ReadInput(
                    StringConverter,
                    Validator<string>((firstName) =>
                    {
                        new ValidatorBuilder()
                            .ValidateFirstName(DefaultValidatorRules.FirstNameMinLen, DefaultValidatorRules.FirstNameMaxLen)
                            .Create()
                            .Validate(new () { FirstName = firstName });
                    }));

            Console.Write("Last name: ");
            personalData.LastName =
                ReadInput(
                    StringConverter,
                    Validator<string>((lastName) =>
                    {
                        new ValidatorBuilder()
                            .ValidateLastName(DefaultValidatorRules.LastNameMinLen, DefaultValidatorRules.LastNameMaxLen)
                            .Create()
                            .Validate(new () { LastName = lastName });
                    }));

            Console.Write("Date of birth: ");
            personalData.DateOfBirth =
                ReadInput(
                    DateTimeConverter(dateTimeFormat),
                    Validator<DateTime>((dateOfBirth) =>
                    {
                        new ValidatorBuilder()
                            .ValidateDateOfBirth(DefaultValidatorRules.DateOfBirthMinValue, DefaultValidatorRules.DateOfBirthMaxValue)
                            .Create()
                            .Validate(new () { DateOfBirth = dateOfBirth });
                    }));

            Console.Write("School grade: ");
            personalData.SchoolGrade =
                ReadInput(
                    NumericConverter<short>,
                    Validator<short>((schoolGrade) =>
                    {
                        new ValidatorBuilder()
                            .ValidateSchoolGrade(DefaultValidatorRules.SchoolGradeMinValue, DefaultValidatorRules.SchoolGradeMaxValue)
                            .Create()
                            .Validate(new () { SchoolGrade = schoolGrade });
                    }));

            Console.Write("Average mark: ");
            personalData.AverageMark =
                ReadInput(
                    NumericConverter<decimal>,
                    Validator<decimal>((averageMark) =>
                    {
                        new ValidatorBuilder()
                            .ValidateAverageMark(DefaultValidatorRules.AverageMarkMinValue, DefaultValidatorRules.AverageMarkMaxValue)
                            .Create()
                            .Validate(new () { AverageMark = averageMark });
                    }));

            Console.Write("Class letter: ");
            personalData.ClassLetter =
                ReadInput(
                    NumericConverter<char>,
                    Validator<char>((classLetter) =>
                    {
                        new ValidatorBuilder()
                            .ValidateAverageMark(DefaultValidatorRules.ClassLetterMinValue, DefaultValidatorRules.ClassLetterMaxValue)
                            .Create()
                            .Validate(new () { ClassLetter = classLetter });
                    }));
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine() !;
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Correct your input:");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);

                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Correct your input:");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Func<T, Tuple<bool, string>> Validator<T>(Action<T> validate)
        {
            return (T input) =>
            {
                try
                {
                    validate(input);
                }
                catch (ArgumentException ex)
                {
                    return new (false, ex.Message);
                }

                return new (true, string.Empty);
            };
        }

        private static Tuple<bool, string, string> StringConverter(string input)
        {
            return new (true, string.Empty, input);
        }

        private static Func<string, Tuple<bool, string, DateTime>> DateTimeConverter(string dateTimeFormat)
        {
            return (string input) =>
            {
                bool success = true;
                string message = string.Empty;
                DateTime res;

                success = DateTime.TryParseExact(
                    input,
                    dateTimeFormat,
                    CultureInfo.InvariantCulture,
                    style: DateTimeStyles.None,
                    out res);

                if (!success)
                {
                    message = $"Invalid value, correct format is \'{dateTimeFormat}\'";
                }

                return new (success, message, res);
            };
        }

        private static Tuple<bool, string, T> NumericConverter<T>(string input)
            where T : struct
        {
            var res = (T?)Convert.ChangeType(input, typeof(T), CultureInfo.InvariantCulture);
            bool success = res is not null;
            string message = success ? string.Empty : "Invalid value";

            return new (success, message, (T)res!);
        }
    }
}
