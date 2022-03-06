using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;
using Models;

namespace FileCabinetApp
{
    /// <summary>
    /// Interacts with user.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Kirill Basenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static int usedValidationRuleIndex;

        private static Tuple<string[], Action<string>>[] args = new Tuple<string[], Action<string>>[]
        {
            new (new[] { "-v", "--validation-rules" }, SetValidationRules),
            new (new[] { "-s", "--storage" }, SetStorage),
        };

        private static Tuple<string, Action>[] storages = new Tuple<string, Action>[]
        {
            new ("memory", SetMemoryService),
            new ("file", SetFileSystemService),
        };

        private static Tuple<string, IRecordValidator>[] validationRules = new Tuple<string, IRecordValidator>[]
        {
            new ("default", new DefaultValidator()),
            new ("custom", new CustomValidator()),
        };

        //private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        //{
        //    new ("help", PrintHelp),
        //    new ("exit", Exit),
        //    new ("stat", Stat),
        //    new ("create", Create),
        //    new ("list", List),
        //    new ("edit", Edit),
        //    new ("find", Find),
        //    new ("import", Import),
        //    new ("export", Export),
        //    new ("remove", Remove),
        //    new ("purge", Purge),
        //};

        internal static IFileCabinetService? FileCabinetService { get; set; }

        internal static bool IsRunning { get; set; } = true;

        internal static IRecordValidator Validator
        {
            get
            {
                return validationRules[usedValidationRuleIndex].Item2;
            }
        }

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="consoleArgs">Arguments passed via console.</param>
        public static void Main(string[] consoleArgs)
        {
            usedValidationRuleIndex = 0;
            SetMemoryService();

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
            Console.WriteLine($"Using {validationRules[usedValidationRuleIndex].Item1} validation rules.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            var handler = CreateCommandHandlers();
            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };

                const int commandIndex = 0;
                const int parametersIndex = 1;

                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                //var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                //if (index >= 0)
                //{
                //    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                //    commands[index].Item2(parameters);
                //}
                //else
                //{
                //    PrintMissedCommandInfo(command);
                //}

                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                try
                {
                    handler.Handle(
                        new AppCommandRequest()
                        {
                            Command = command,
                            Parameters = parameters,
                        });
                }
                catch (InvalidOperationException)
                {
                    PrintMissedCommandInfo(command);
                }

                Console.WriteLine();
            }
            while (IsRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
        }

        private static void ProceedArgs(string[] consoleArgs)
        {
            Stack<string> paramsAndArgs = new ();
            int index;
            foreach (var consoleArg in consoleArgs.Reverse())
            {
                if ((index = consoleArg.IndexOf('=', StringComparison.Ordinal)) != -1)
                {
                    paramsAndArgs.Push(consoleArg[(index + 1)..]);
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

        private static void SetValidationRules(string rule)
        {
            int index;
            if ((index = Array.FindIndex(validationRules, 0, validationRules.Length, i => i.Item1.Equals(rule, StringComparison.InvariantCultureIgnoreCase))) != -1)
            {
                usedValidationRuleIndex = index;
            }
            else
            {
                throw new ArgumentException($"No defined rule \'{rule}\'.");
            }
        }

        private static void SetStorage(string storage)
        {
            int index;
            if ((index = Array.FindIndex(storages, 0, storages.Length, i => i.Item1.Equals(storage, StringComparison.InvariantCultureIgnoreCase))) != -1)
            {
                storages[index].Item2();
            }
            else
            {
                throw new ArgumentException($"No defined storage \'{storage}\'");
            }
        }

        private static void SetMemoryService()
        {
            FileCabinetService = new FileCabinetMemoryService(validationRules[usedValidationRuleIndex].Item2);
        }

        private static void SetFileSystemService()
        {
            var fileName = "cabinet-records.db";
            var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            FileCabinetService = new FileCabinetFileSystemService(fileStream, validationRules[usedValidationRuleIndex].Item2);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            return new CommandHandler();
        }

        //private static void PrintHelp(string parameters)
        //{
        //    if (!string.IsNullOrEmpty(parameters))
        //    {
        //        var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
        //        if (index >= 0)
        //        {
        //            Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"There is no explanation for '{parameters}' command.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Available commands:");

        //        foreach (var helpMessage in helpMessages)
        //        {
        //            Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
        //        }
        //    }

        //    Console.WriteLine();
        //}

        //private static void Exit(string parameters)
        //{
        //    Console.WriteLine("Exiting an application...");
        //    IsRunning = false;
        //}

        //private static void Stat(string parameters)
        //{
        //    var recordsCount = Program.FileCabinetService!.GetStat();
        //    Console.WriteLine($"{recordsCount.have} record(s) total.");
        //    Console.WriteLine($"{recordsCount.deleted} record(s) deleted.");
        //}

        //private static void Create(string parameters)
        //{
        //    while (IsRunning)
        //    {
        //        try
        //        {
        //            ReadRecordDataFromConsole(FileCabinetRecord.InputDateTimeFormat, out var personalData);

        //            Console.WriteLine(
        //                "Record #{0} is created.",
        //                Program.FileCabinetService!.CreateRecord(personalData));
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
        //            continue;
        //        }

        //        break;
        //    }
        //}

        //private static void Edit(string parameters)
        //{
        //    int id;

        //    while (IsRunning)
        //    {
        //        try
        //        {
        //            Console.Write("id: ");
        //            if (!int.TryParse(Console.ReadLine(), out id))
        //            {
        //                throw new ArgumentException("Invalid input for id.");
        //            }

        //            ReadRecordDataFromConsole(FileCabinetRecord.OutputDateTimeFormat, out var personalData);

        //            FileCabinetService!.EditRecord(id, personalData);
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
        //            continue;
        //        }

        //        Console.WriteLine($"Record #{id} is updated.");
        //        break;
        //    }
        //}

        //private static void Find(string parameters)
        //{
        //    try
        //    {
        //        const int firstGroupMatchIndex = 1;
        //        const int secondGroupMatchIndex = 2;

        //        const string regexPattern = @"^\s*(\w+)\s+""(\d{4}-\w{3}-\d{2}|\w+)""\s*$";
        //        var regex = new Regex(regexPattern);

        //        if (!regex.IsMatch(parameters))
        //        {
        //            throw new ArgumentException("Invalid parameters input, see help.");
        //        }

        //        string fieldName;
        //        string stringValue;

        //        var match = regex.Match(parameters);

        //        fieldName = match.Groups[firstGroupMatchIndex].Value;
        //        stringValue = match.Groups[secondGroupMatchIndex].Value;

        //        var found = FileCabinetService!.FindByField(fieldName, stringValue);
        //        if (found.Count > 0)
        //        {
        //            foreach (var record in found)
        //            {
        //                Console.WriteLine(record);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No records found.");
        //        }
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //private static void List(string parameters)
        //{
        //    foreach (var record in FileCabinetService!.GetRecords())
        //    {
        //        Console.WriteLine(record);
        //    }
        //}

        //private static void ReadRecordDataFromConsole(string dateTimeFormat, out PersonalData personalData)
        //{
        //    personalData = new ();
        //    var usedValidationRule = validationRules[usedValidationRuleIndex].Item2;

        //    Console.Write("First name: ");
        //    personalData.FirstName =
        //        ReadInput(
        //            StringConverter,
        //            Validator<string>(usedValidationRule.ValidateFirstName));

        //    Console.Write("Last name: ");
        //    personalData.LastName =
        //        ReadInput(
        //            StringConverter,
        //            Validator<string>(usedValidationRule.ValidateLastName));

        //    Console.Write("Date of birth: ");
        //    personalData.DateOfBirth =
        //        ReadInput(
        //            DateTimeConverter(dateTimeFormat),
        //            Validator<DateTime>(usedValidationRule.ValidateDateOfBirth));

        //    Console.Write("School grade: ");
        //    personalData.SchoolGrade =
        //        ReadInput(
        //            NumericConverter<short>,
        //            Validator<short>(usedValidationRule.ValidateSchoolGrade));

        //    Console.Write("Average mark: ");
        //    personalData.AverageMark =
        //        ReadInput(
        //            NumericConverter<decimal>,
        //            Validator<decimal>(usedValidationRule.ValidateAverageMark));

        //    Console.Write("Class letter: ");
        //    personalData.ClassLetter =
        //        ReadInput(
        //            NumericConverter<char>,
        //            Validator<char>(usedValidationRule.ValidateClassLetter));
        //}

        //private static Func<T, Tuple<bool, string>> Validator<T>(Action<T> validate)
        //{
        //    return (T input) =>
        //    {
        //        try
        //        {
        //            validate(input);
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            return new Tuple<bool, string>(false, ex.Message);
        //        }

        //        return new (true, string.Empty);
        //    };
        //}

        //private static Tuple<bool, string, string> StringConverter(string input)
        //{
        //    return new (true, string.Empty, input);
        //}

        //private static Func<string, Tuple<bool, string, DateTime>> DateTimeConverter(string dateTimeFormat)
        //{
        //    return (string input) =>
        //    {
        //        bool success = true;
        //        string message = string.Empty;
        //        DateTime res;

        //        success = DateTime.TryParseExact(
        //            input,
        //            dateTimeFormat,
        //            CultureInfo.InvariantCulture,
        //            DateTimeStyles.None,
        //            out res);

        //        if (!success)
        //        {
        //            message = $"Invalid value, correct format is \'{dateTimeFormat}\'";
        //        }

        //        return new (success, message, res);
        //    };
        //}

        //private static Tuple<bool, string, T> NumericConverter<T>(string input)
        //    where T : struct
        //{
        //    var res = (T?)Convert.ChangeType(input, typeof(T), CultureInfo.InvariantCulture);
        //    bool success = res is not null;
        //    string message = success ? string.Empty : "Invalid value";

        //    return new (success, message, (T)res!);
        //}

        //private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        //{
        //    do
        //    {
        //        T value;

        //        var input = Console.ReadLine() !;
        //        var conversionResult = converter(input);

        //        if (!conversionResult.Item1)
        //        {
        //            Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Correct your input:");
        //            continue;
        //        }

        //        value = conversionResult.Item3;

        //        var validationResult = validator(value);
        //        if (!validationResult.Item1)
        //        {
        //            Console.WriteLine($"Validation failed: {validationResult.Item2}. Correct your input:");
        //            continue;
        //        }

        //        return value;
        //    }
        //    while (true);
        //}

        //private static void SetValidationRules(string rule)
        //{
        //    int index;
        //    if ((index = Array.FindIndex(validationRules, 0, validationRules.Length, i => i.Item1.Equals(rule, StringComparison.InvariantCultureIgnoreCase))) != -1)
        //    {
        //        usedValidationRuleIndex = index;
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"No defined rule \'{rule}\'.");
        //    }
        //}

        //private static void SetStorage(string storage)
        //{
        //    int index;
        //    if ((index = Array.FindIndex(storages, 0, storages.Length, i => i.Item1.Equals(storage, StringComparison.InvariantCultureIgnoreCase))) != -1)
        //    {
        //        storages[index].Item2();
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"No defined storage \'{storage}\'");
        //    }
        //}

        //private static void SetMemoryService()
        //{
        //    FileCabinetService = new FileCabinetMemoryService(validationRules[usedValidationRuleIndex].Item2);
        //}

        //private static void SetFileSystemService()
        //{
        //    var fileName = "cabinet-records.db";
        //    var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        //    FileCabinetService = new FileCabinetFileSystemService(fileStream, validationRules[usedValidationRuleIndex].Item2);
        //}

        //private static void Export(string parameters)
        //{
        //    var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

        //    if (spaceIndex == -1)
        //    {
        //        throw new ArgumentException("Invalid export parameters");
        //    }

        //    var extension = parameters[..spaceIndex];
        //    var filePath = parameters[(spaceIndex + 1) ..];

        //    using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        //    var snapshot = FileCabinetService!.MakeSnapshot();

        //    switch (extension)
        //    {
        //        case "csv":
        //            {
        //                snapshot.SaveToCsv(writer);
        //                break;
        //            }

        //        case "xml":
        //            {
        //                snapshot.SaveToXml(writer);
        //                break;
        //            }

        //        default:
        //            {
        //                throw new ArgumentException($"Extension {extension} is unsupportable.");
        //            }
        //    }
        //}

        //private static void Import(string parameters)
        //{
        //    var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

        //    if (spaceIndex == -1)
        //    {
        //        throw new ArgumentException("Invalid export parameters");
        //    }

        //    var extension = parameters[..spaceIndex];
        //    var filePath = parameters[(spaceIndex + 1) ..];

        //    if (!File.Exists(filePath))
        //    {
        //        throw new ArgumentException("File does not exist.");
        //    }

        //    using var writer = new StreamReader(filePath, Encoding.UTF8);

        //    switch (extension)
        //    {
        //        case "csv":
        //            {
        //                ImportCsv(filePath);
        //                break;
        //            }

        //        case "xml":
        //            {
        //                ImportXml(filePath);
        //                break;
        //            }

        //        default:
        //            {
        //                throw new ArgumentException($"Extension {extension} is unsupportable.");
        //            }
        //    }
        //}

        //private static void ImportCsv(string filePath)
        //{
        //    using var reader = new StreamReader(filePath, Encoding.UTF8);
        //    var snapshot = new FileCabinetServiceSnapshot();
        //    snapshot.LoadFromCsv(reader);
        //    FileCabinetService!.Restore(snapshot);
        //}

        //private static void ImportXml(string filePath)
        //{
        //    using var reader = new StreamReader(filePath, Encoding.UTF8);
        //    var snapshot = new FileCabinetServiceSnapshot();
        //    snapshot.LoadFromXml(reader);
        //    FileCabinetService!.Restore(snapshot);
        //}

        //private static void Remove(string parameters)
        //{
        //    if (!int.TryParse(parameters, out var id))
        //    {
        //        throw new ArgumentException($"Cannot parse id \'{parameters}\'.");
        //    }

        //    bool deleted = FileCabinetService!.Remove(id);
        //    Console.WriteLine($"Record #{id} {(deleted ? "is removed" : "does not exists")}.");
        //}

        //private static void Purge(string parameters)
        //{
        //    int was = FileCabinetService!.GetStat().have;
        //    int deleted = FileCabinetService!.Purge();
        //    Console.WriteLine($"{deleted} of {was} records were purged.");
        //}
    }
}
