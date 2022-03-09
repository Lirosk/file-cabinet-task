using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.ExactCommandHandlers;
using FileCabinetApp.Extensions;
using FileCabinetApp.Meters;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;

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

        private static readonly string ValidationRulesFile = "validation-rules.json";

        private static string validationRulesNaming = "default";

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

        private static IFileCabinetService? fileCabinetService;

        private static bool isRunning = true;

        private static IRecordValidator? validator;

        /// <summary>
        /// Gets encodind used in this app.
        /// </summary>
        /// <value>Encodind used in this app.</value>
        public static Encoding EncodingUsed { get; private set; } = Encoding.Unicode;

        /// <summary>
        /// Gets validation rules used in this app.
        /// </summary>
        /// <value>Validation rules used in this app.</value>
        public static ValidationRules? ValidationRules { get; private set; }

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="consoleArgs">Arguments passed via console.</param>
        public static void Main(string[] consoleArgs)
        {
            try
            {
                DoStartupStaff(consoleArgs);
                fileCabinetService = new ServiceMeter(fileCabinetService!);
                DoFileCabinetStaff();
            }
            catch (Exception)
            {
                Console.WriteLine($"Oops, internal error. Exiting app.{Environment.NewLine}");
                return;
            }
        }

        private static void DoFileCabinetStaff()
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using {validationRulesNaming} validation rules.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            var handlers = CreateCommandHandlers();
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

                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                try
                {
                    handlers.Handle(
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
            while (isRunning);
        }

        private static void DoStartupStaff(string[] consoleArgs)
        {
            SetDefaults();
            ProceedConfiguration();

            try
            {
                ProceedArgs(consoleArgs);
                SetValidator(ValidationRules!);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                return;
            }
        }

        private static void ProceedConfiguration()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            if (!File.Exists(Path.Combine(currentDirectory, ValidationRulesFile)))
            {
                throw new FileNotFoundException(ValidationRulesFile);
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(ValidationRulesFile)
                .Build();

            var configuration = config.GetSection(validationRulesNaming);
            ValidationRules = configuration.Get<ValidationRules>();

            if (ValidationRules is null)
            {
                throw new InvalidOperationException("Invalid configuration file for validation rules.");
            }
        }

        private static void SetDefaults()
        {
            validationRulesNaming = "default";
            SetMemoryService();
        }

        private static void SetValidator(ValidationRules rules)
        {
            validator = new ValidatorBuilder()
                .ValidateFirstName(rules.FirstName.Min, rules.FirstName.Max)
                .ValidateLastName(rules.LastName.Min, rules.LastName.Max)
                .ValidateDateOfBirth(rules.DateOfBirth.Min, rules.DateOfBirth.Max)
                .ValidateSchoolGrade(rules.SchoolGrade.Min, rules.SchoolGrade.Max)
                .ValidateAverageMark(rules.AverageMark.Min, rules.AverageMark.Max)
                .ValidateClassLetter(rules.ClassLetter.Min, rules.ClassLetter.Max)
                .Create();
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

        private static void SetValidationRules(string rule)
        {
            validationRulesNaming = rule;
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
            fileCabinetService = new FileCabinetMemoryService(validator!);
        }

        private static void SetFileSystemService()
        {
            var fileName = "cabinet-records.db";
            var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fileCabinetService = new FileCabinetFileSystemService(fileStream, validator!);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var printer = (IEnumerable<FileCabinetRecord> records) =>
            {
                foreach (var record in records)
                {
                    Console.WriteLine(record);
                }
            };

            ICommandHandler handlers = new CreateCommandHandler(fileCabinetService!);

            var editHandler = new EditCommandHandler(fileCabinetService!);
            var exitHandler = new ExitCommandHandler(fileCabinetService!, (running) => isRunning = running);
            var exportHandler = new ExportCommandHandler(fileCabinetService!);
            var findHandler = new FindCommandHandler(fileCabinetService!, printer);
            var helpHandler = new HelpCommandHandler();
            var importHandler = new ImportCommandHandler(fileCabinetService!);
            var listHandler = new ListCommandHandler(fileCabinetService!, printer);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService!);
            var removeHandler = new RemoveCommandHandler(fileCabinetService!);
            var statHandler = new StatCommandHandler(fileCabinetService!);

            handlers.SetNext(editHandler);
            editHandler.SetNext(exitHandler);
            exitHandler.SetNext(exportHandler);
            exportHandler.SetNext(findHandler);
            findHandler.SetNext(helpHandler);
            helpHandler.SetNext(importHandler);
            importHandler.SetNext(listHandler);
            listHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(removeHandler);
            removeHandler.SetNext(statHandler);

            return handlers;
        }
    }
}
