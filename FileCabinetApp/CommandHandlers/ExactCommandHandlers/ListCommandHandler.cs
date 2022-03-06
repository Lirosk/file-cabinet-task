using FileCabinetApp.RecordPrinters;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer)
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
            this.printer.Print(records);
        }
    }
}
