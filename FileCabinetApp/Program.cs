using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.ExactCommandHandlers;
using FileCabinetApp.RecordPrinters;
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
        public static Encoding EncodingUsed { get; private set; } = Encoding.Unicode;

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

        private static IFileCabinetService? fileCabinetService;

        private static bool isRunning = true;

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
            fileCabinetService = new FileCabinetMemoryService(validationRules[usedValidationRuleIndex].Item2);
        }

        private static void SetFileSystemService()
        {
            var fileName = "cabinet-records.db";
            var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fileCabinetService = new FileCabinetFileSystemService(fileStream, validationRules[usedValidationRuleIndex].Item2);
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
            handlers.SetNext(new EditCommandHandler(fileCabinetService!));
            handlers.SetNext(new ExitCommandHandler(fileCabinetService!, (running) => isRunning = running));
            handlers.SetNext(new ExportCommandHandler(fileCabinetService!));
            handlers.SetNext(new FindCommandHandler(fileCabinetService!, printer));
            handlers.SetNext(new HelpCommandHandler(fileCabinetService!));
            handlers.SetNext(new ImportCommandHandler(fileCabinetService!));
            handlers.SetNext(new ListCommandHandler(fileCabinetService!, printer));
            handlers.SetNext(new PurgeCommandHandler(fileCabinetService!));
            handlers.SetNext(new RemoveCommandHandler(fileCabinetService!));
            handlers.SetNext(new StatCommandHandler(fileCabinetService!));

            return handlers;
        }
    }
}
