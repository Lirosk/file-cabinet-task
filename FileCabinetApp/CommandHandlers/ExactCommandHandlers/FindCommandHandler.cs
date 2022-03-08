using System.Text.RegularExpressions;

using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the find command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        /// <param name="printer">Prints the records in its own way.</param>
        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Handles the find command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
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
                    this.printer(found);
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
