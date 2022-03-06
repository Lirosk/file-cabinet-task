using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    internal class ListCommandHandler : ServiceCommandHandlerBase
    {
        public ListCommandHandler(IFileCabinetService service)
            : base("list", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.List(request.Parameters);
        }

        private void List(string parameters)
        {
            foreach (var record in this.Service.GetRecords())
            {
                Console.WriteLine(record);
            }
        }
    }
}
