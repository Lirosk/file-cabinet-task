using System.Globalization;
using System.Text.RegularExpressions;

using FileCabinetApp.FileCabinetServices;

namespace FileCabinetApp
{
    /// <summary>
    /// Interacts with user.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Kirill Basenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static FileCabinetService fileCabinetService = new FileCabinetDefaultService();

        private static string usingService = string.Empty;

        private static Tuple<string[], Action<string>>[] args = new Tuple<string[], Action<string>>[]
        {
            new (new[] { "-v", "--validation-rules" }, SetValidationRules),
        };

        private static Tuple<string, FileCabinetService>[] validationRules = new Tuple<string, FileCabinetService>[]
        {
            new ("default", new FileCabinetDefaultService()),
            new ("custom", new FileCabinetCustomService()),
        };

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

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="consoleArgs">Arguments passed via console.</param>
        public static void Main(string[] consoleArgs)
        {
            try
            {
                ProceedArgs(consoleArgs);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine($"Error: Invalid args input.{Environment.NewLine}");
                return;
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using {usingService} validation rules.");
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
                    ReadRecordDataFromConsole(out var personalData);

                    Console.WriteLine(
                        "Record #{0} is created.",
                        Program.fileCabinetService.CreateRecord(personalData));
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

            while (isRunning)
            {
                try
                {
                    Console.Write("id: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        throw new ArgumentException("Invalid input for id.");
                    }

                    ReadRecordDataFromConsole(out var personalData);

                    fileCabinetService.EditRecord(id, personalData);
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

        private static void ReadRecordDataFromConsole(out PersonalData personalData)
        {
            personalData = new ();
            string dateOfBirthString;
            string schoolGradeString;
            string averageMarkString;
            string classLetterString;

            Console.Write("First name: ");
            personalData.FirstName = Console.ReadLine() !;

            Console.Write("Last name: ");
            personalData.LastName = Console.ReadLine() !;

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
                out var dateOfBirth))
            {
                throw new ArgumentException($"Correct date format: {FileCabinetRecord.InputDateTimeFormat}.");
            }

            if (!short.TryParse(schoolGradeString, out var schoolGrade))
            {
                throw new ArgumentException("Invalid input for school grade.");
            }

            if (!decimal.TryParse(averageMarkString, out var averageMark))
            {
                throw new ArgumentException("Invalid input for average mark.");
            }

            if (!char.TryParse(classLetterString, out var classLetter))
            {
                throw new ArgumentException("Invalid input for class letter.");
            }

            personalData.DateOfBirth = dateOfBirth;
            personalData.SchoolGrade = schoolGrade;
            personalData.AverageMark = averageMark;
            personalData.ClassLetter = classLetter;
        }

        private static void SetValidationRules(string rule)
        {
            int index;
            if ((index = Array.FindIndex(validationRules, 0, validationRules.Length, i => i.Item1.Equals(rule, StringComparison.InvariantCultureIgnoreCase))) != -1)
            {
                fileCabinetService = validationRules[index].Item2;
                usingService = validationRules[index].Item1;
            }
            else
            {
                throw new ArgumentException($"No defined rule \'{rule}\'.");
            }
        }

        private static void ProceedArgs(string[] consoleArgs)
        {
            Stack<string> paramsAndArgs = new ();
            int index;
            foreach (var consoleArg in consoleArgs.Reverse())
            {
                if ((index = consoleArg.IndexOf('=', StringComparison.Ordinal)) != -1)
                {
                    paramsAndArgs.Push(consoleArg[(index + 1) ..]);
                    paramsAndArgs.Push(consoleArg[..index]);
                    continue;
                }

                paramsAndArgs.Push(consoleArg);
            }

            string param, arg;
            while (paramsAndArgs.TryPop(out param!))
            {
                if ((index = Array.FindIndex(args, 0, i => i.Item1.Contains(param))) != -1)
                {
                    if (paramsAndArgs.TryPop(out arg!))
                    {
                        args[index].Item2(arg);
                    }
                    else
                    {
                        throw new ArgumentException($"No argument for \'{param}\' parameter");
                    }
                }
                else
                {
                    throw new ArgumentException($"No defined parameter \'{param}\'.");
                }
            }
        }
    }
}
