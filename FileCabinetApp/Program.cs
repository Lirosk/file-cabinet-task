﻿using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.ExactCommandHandlers;
using FileCabinetApp.Extensions;
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

        private static Tuple<string, Action>[] validationRules = new Tuple<string, Action>[]
        {
            new ("default", SetDefaultValidator),
            new ("custom", SetCustomValidator),
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
        /// Entry point.
        /// </summary>
        /// <param name="consoleArgs">Arguments passed via console.</param>
        public static void Main(string[] consoleArgs)
        {
            usedValidationRuleIndex = 0;
            validationRules[usedValidationRuleIndex].Item2();
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

        private static void SetDefaultValidator()
        {
            validator = new ValidatorBuilder().CreateDefault();
        }

        private static void SetCustomValidator()
        {
            validator = new ValidatorBuilder().CreateCustom();
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
