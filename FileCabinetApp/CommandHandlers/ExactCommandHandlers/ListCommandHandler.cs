using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

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
