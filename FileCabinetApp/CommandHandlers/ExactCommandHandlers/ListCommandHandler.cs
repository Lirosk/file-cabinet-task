using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the list command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        /// <param name="printer">Prints the records in its own way.</param>
        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Handles the list command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
        protected override void Handle(AppCommandRequest request)
        {
            this.List(request.Parameters);
        }

        private void List(string parameters)
        {
            var records = this.Service.GetRecords();
            this.printer(records);
        }
    }
}
