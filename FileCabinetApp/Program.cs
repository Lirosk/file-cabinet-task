using System.Globalization;

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
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "prints total count of records" },
            new string[] { "create", "create new record" },
            new string[] { "list", "prints all records" },
            new string[] { "edit", "edit existring record via id" },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
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
            string firstName,
                   lastName;
            DateTime dateOfBirth;
            short schoolGrade;
            decimal averageMark;
            char classLetter;

            while (isRunning)
            {
                try
                {
                    Console.Write("First name: ");
                    firstName = Console.ReadLine() !;

                    Console.Write("Last name: ");
                    lastName = Console.ReadLine() !;

                    Console.Write("Date of birth: ");
                    if (!DateTime.TryParseExact(
                        Console.ReadLine(),
                        "MM/dd/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dateOfBirth))
                    {
                        throw new ArgumentException("Correct date format: month/day/year.");
                    }

                    Console.Write("School grade: ");
                    if (!short.TryParse(Console.ReadLine(), out schoolGrade))
                    {
                        throw new ArgumentException("Invalid input for school grade.");
                    }

                    Console.Write("Average mark: ");
                    if (!decimal.TryParse(Console.ReadLine(), out averageMark))
                    {
                        throw new ArgumentException("Invalid input for average mark.");
                    }

                    Console.Write("Class letter: ");
                    if (!char.TryParse(Console.ReadLine(), out classLetter))
                    {
                        throw new ArgumentException("Invalid input for class letter.");
                    }

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
            string firstName,
                   lastName;
            DateTime dateOfBirth;
            short schoolGrade;
            decimal averageMark;
            char classLetter;
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

                    Console.Write("First name: ");
                    firstName = Console.ReadLine() !;

                    Console.Write("Last name: ");
                    lastName = Console.ReadLine() !;

                    Console.Write("Date of birth: ");
                    if (!DateTime.TryParseExact(
                            Console.ReadLine(),
                            "MM/dd/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out dateOfBirth))
                    {
                        throw new ArgumentException("Correct date format: month/day/year.");
                    }

                    Console.Write("School grade: ");
                    if (!short.TryParse(Console.ReadLine(), out schoolGrade))
                    {
                        throw new ArgumentException("Invalid input for school grade.");
                    }

                    Console.Write("Average mark: ");
                    if (!decimal.TryParse(Console.ReadLine(), out averageMark))
                    {
                        throw new ArgumentException("Invalid input for average mark.");
                    }

                    Console.Write("Class letter: ");
                    if (!char.TryParse(Console.ReadLine(), out classLetter))
                    {
                        throw new ArgumentException("Invalid input for class letter.");
                    }

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

        private static void List(string parameters)
        {
            foreach (var record in Program.fileCabinetService.GetRecords())
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.SchoolGrade}, {record.AverageMark}, {record.ClassLetter}");
            }
        }
    }
}
