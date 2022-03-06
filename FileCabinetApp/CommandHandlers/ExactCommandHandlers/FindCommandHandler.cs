using FileCabinetApp.RecordPrinters;
using FileCabinetApp.Services;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Find(request.Parameters);
        }

        private void Find(string parameters)
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

                var found = this.Service.FindByField(fieldName, stringValue);
                if (found.Count > 0)
                {
                    this.printer.Print(found);
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
