using FileCabinetApp.Services;
using Models;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "stat", "prints total count of records", "The 'stat' command prints statistics about stored records." },
            new[] { "create", $"create new record, datetime format: {FileCabinetRecord.InputDateTimeFormat}", "The 'create' command creates new record with given by user data." },
            new[] { "list", "prints all records", "The 'list' command prints all records." },
            new[] { "edit", $"edit existring record via id, datetime format: {FileCabinetRecord.InputDateTimeFormat}", "The 'edit' command edit record with given by user id with given by user data." },
            new[] { "find", $"find records by field value, format: 'find fieldname \"value\"', datetime format: {FileCabinetRecord.OutputDateTimeFormat}", "The 'find' command finds id of record with given by user field and its value." },
            new[] { "export", "saves records to the specified file", "The 'export' command saves records to file with given by user path." },
            new[] { "import", "imports records from file", "The 'import' command imports records from file with given by user path." },
            new[] { "remove", "remove record with given id", "The 'remove' command marks records with given by user id as deleted." },
            new[] { "purge", "remove records marked as deleted", "The 'purge' command purges all records marked as deleted." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.FileCabinetService!.GetStat();
            Console.WriteLine($"{recordsCount.have} record(s) total.");
            Console.WriteLine($"{recordsCount.deleted} record(s) deleted.");
        }

        private static void Create(string parameters)
        {
            while (Program.IsRunning)
            {
                try
                {
                    ReadRecordDataFromConsole(FileCabinetRecord.InputDateTimeFormat, out var personalData);

                    Console.WriteLine(
                        "Record #{0} is created.",
                        Program.FileCabinetService!.CreateRecord(personalData));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                    continue;
                }

                break;
            }
        }

        private static void Edit(string parameters)
        {
            int id;

            while (Program.IsRunning)
            {
                try
                {
                    Console.Write("id: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        throw new ArgumentException("Invalid input for id.");
                    }

                    ReadRecordDataFromConsole(FileCabinetRecord.OutputDateTimeFormat, out var personalData);

                    Program.FileCabinetService!.EditRecord(id, personalData);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                    continue;
                }

                Console.WriteLine($"Record #{id} is updated.");
                break;
            }
        }

        private static void Find(string parameters)
        {
            try
            {
                const int firstGroupMatchIndex = 1;
                const int secondGroupMatchIndex = 2;

                const string regexPattern = @"^\s*(\w+)\s+""(\d{4}-\w{3}-\d{2}|\w+)""\s*$";
                var regex = new Regex(regexPattern);

                if (!regex.IsMatch(parameters))
                {
                    throw new ArgumentException("Invalid parameters input, see help.");
                }

                string fieldName;
                string stringValue;

                var match = regex.Match(parameters);

                fieldName = match.Groups[firstGroupMatchIndex].Value;
                stringValue = match.Groups[secondGroupMatchIndex].Value;

                var found = Program.FileCabinetService!.FindByField(fieldName, stringValue);
                if (found.Count > 0)
                {
                    foreach (var record in found)
                    {
                        Console.WriteLine(record);
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void List(string parameters)
        {
            foreach (var record in Program.FileCabinetService!.GetRecords())
            {
                Console.WriteLine(record);
            }
        }

        private static void ReadRecordDataFromConsole(string dateTimeFormat, out PersonalData personalData)
        {
            personalData = new();
            var usedValidationRule = Program.Validator;

            Console.Write("First name: ");
            personalData.FirstName =
                ReadInput(
                    StringConverter,
                    Validator<string>(usedValidationRule.ValidateFirstName));

            Console.Write("Last name: ");
            personalData.LastName =
                ReadInput(
                    StringConverter,
                    Validator<string>(usedValidationRule.ValidateLastName));

            Console.Write("Date of birth: ");
            personalData.DateOfBirth =
                ReadInput(
                    DateTimeConverter(dateTimeFormat),
                    Validator<DateTime>(usedValidationRule.ValidateDateOfBirth));

            Console.Write("School grade: ");
            personalData.SchoolGrade =
                ReadInput(
                    NumericConverter<short>,
                    Validator<short>(usedValidationRule.ValidateSchoolGrade));

            Console.Write("Average mark: ");
            personalData.AverageMark =
                ReadInput(
                    NumericConverter<decimal>,
                    Validator<decimal>(usedValidationRule.ValidateAverageMark));

            Console.Write("Class letter: ");
            personalData.ClassLetter =
                ReadInput(
                    NumericConverter<char>,
                    Validator<char>(usedValidationRule.ValidateClassLetter));
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
                    DateTimeStyles.None,
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

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine()!;
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

        private static void Export(string parameters)
        {
            var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

            if (spaceIndex == -1)
            {
                throw new ArgumentException("Invalid export parameters");
            }

            var extension = parameters[..spaceIndex];
            var filePath = parameters[(spaceIndex + 1)..];

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            var snapshot = Program.FileCabinetService!.MakeSnapshot();

            switch (extension)
            {
                case "csv":
                    {
                        snapshot.SaveToCsv(writer);
                        break;
                    }

                case "xml":
                    {
                        snapshot.SaveToXml(writer);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException($"Extension {extension} is unsupportable.");
                    }
            }
        }

        private static void Import(string parameters)
        {
            var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

            if (spaceIndex == -1)
            {
                throw new ArgumentException("Invalid export parameters");
            }

            var extension = parameters[..spaceIndex];
            var filePath = parameters[(spaceIndex + 1)..];

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File does not exist.");
            }

            using var writer = new StreamReader(filePath, Encoding.UTF8);

            switch (extension)
            {
                case "csv":
                    {
                        ImportCsv(filePath);
                        break;
                    }

                case "xml":
                    {
                        ImportXml(filePath);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException($"Extension {extension} is unsupportable.");
                    }
            }
        }

        private static void ImportCsv(string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var snapshot = new FileCabinetServiceSnapshot();
            snapshot.LoadFromCsv(reader);
            Program.FileCabinetService!.Restore(snapshot);
        }

        private static void ImportXml(string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var snapshot = new FileCabinetServiceSnapshot();
            snapshot.LoadFromXml(reader);
            Program.FileCabinetService!.Restore(snapshot);
        }

        private static void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                throw new ArgumentException($"Cannot parse id \'{parameters}\'.");
            }

            bool deleted = Program.FileCabinetService!.Remove(id);
            Console.WriteLine($"Record #{id} {(deleted ? "is removed" : "does not exists")}.");
        }

        private static void Purge(string parameters)
        {
            int was = Program.FileCabinetService!.GetStat().have;
            int deleted = Program.FileCabinetService!.Purge();
            Console.WriteLine($"{deleted} of {was} records were purged.");
        }
    }
}
