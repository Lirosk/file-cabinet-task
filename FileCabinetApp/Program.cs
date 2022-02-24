using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Kirill Basenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static FileCabinetService fileCabinetService = new ();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new ("help", PrintHelp),
            new ("exit", Exit),
            new ("stat", Stat),
            new ("create", Create),
            new ("list", List),
            new ("edit", Edit),
            new ("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "stat", "prints total count of records" },
            new[] { "create", $"create new record, datetime format: {FileCabinetRecord.InputDateTimeFormat}" },
            new[] { "list", "prints all records" },
            new[] { "edit", $"edit existring record via id, datetime format: {FileCabinetRecord.InputDateTimeFormat}" },
            new[] { "find", $"find records by field value, format: 'find fieldname \"value\"', datetime format: {FileCabinetRecord.OutputDateTimeFormat}" },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }

                Console.WriteLine();
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            while (isRunning)
            {
                try
                {
                    ReadRecordDataFromConsole(
                        out var firstName,
                        out var lastName,
                        out var dateOfBirth,
                        out var schoolGrade,
                        out var averageMark,
                        out var classLetter);

                    Console.WriteLine(
                        "Record #{0} is created.",
                        Program.fileCabinetService.CreateRecord(
                            firstName!,
                            lastName!,
                            dateOfBirth,
                            schoolGrade,
                            averageMark,
                            classLetter));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine);
                    continue;
                }

                break;
            }
        }

        private static void Edit(string parameters)
        {
            int id;

            while (isRunning)
            {
                try
                {
                    Console.Write("id: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        throw new ArgumentException("Invalid input for id.");
                    }

                    ReadRecordDataFromConsole(
                        out var firstName,
                        out var lastName,
                        out var dateOfBirth,
                        out var schoolGrade,
                        out var averageMark,
                        out var classLetter);

                    fileCabinetService.EditRecord(
                        id,
                        firstName,
                        lastName,
                        dateOfBirth,
                        schoolGrade,
                        averageMark,
                        classLetter);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine);
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

                var found = fileCabinetService.FindByField(fieldName, stringValue);
                if (found.Length > 0)
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
            foreach (var record in Program.fileCabinetService.GetRecords())
            {
                Console.WriteLine(record);
            }
        }

        private static void ReadRecordDataFromConsole(
            out string firstName,
            out string lastName,
            out DateTime dateOfBirth,
            out short schoolGrade,
            out decimal averageMark,
            out char classLetter)
        {
            string dateOfBirthString;
            string schoolGradeString;
            string averageMarkString;
            string classLetterString;

            Console.Write("First name: ");
            firstName = Console.ReadLine() !;

            Console.Write("Last name: ");
            lastName = Console.ReadLine() !;

            Console.Write("Date of birth: ");
            dateOfBirthString = Console.ReadLine() !;

            Console.Write("School grade: ");
            schoolGradeString = Console.ReadLine() !;

            Console.Write("Average mark: ");
            averageMarkString = Console.ReadLine() !;

            Console.Write("Class letter: ");
            classLetterString = Console.ReadLine() !;

            if (!DateTime.TryParseExact(
                dateOfBirthString,
                FileCabinetRecord.InputDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateOfBirth))
            {
                throw new ArgumentException($"Correct date format: {FileCabinetRecord.InputDateTimeFormat}.");
            }

            if (!short.TryParse(schoolGradeString, out schoolGrade))
            {
                throw new ArgumentException("Invalid input for school grade.");
            }

            if (!decimal.TryParse(averageMarkString, out averageMark))
            {
                throw new ArgumentException("Invalid input for average mark.");
            }

            if (!char.TryParse(classLetterString, out classLetter))
            {
                throw new ArgumentException("Invalid input for class letter.");
            }
        }
    }
}
