using FileCabinetApp.Services;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class FindCommandHandler : CommandHandlerBase
    {
        public FindCommandHandler(IFileCabinetService service)
            : base("find", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Find(request.Parameters);
        }

        private static void Find(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

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

                var found = Program.FileCabinetService.FindByField(fieldName, stringValue);
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
    }
}
