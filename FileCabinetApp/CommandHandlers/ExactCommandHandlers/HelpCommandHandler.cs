using Models;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
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

        public HelpCommandHandler()
            : base("help")
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Help(request.Parameters);
        }

        private static void Help(string parameters)
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
    }
}
