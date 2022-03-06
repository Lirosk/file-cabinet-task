using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    internal class ListCommandHandler : CommandHandlerBase
    {
        public ListCommandHandler(IFileCabinetService service)
            : base("list", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            List(request.Parameters);
        }

        private static void List(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            foreach (var record in Program.FileCabinetService.GetRecords())
            {
                Console.WriteLine(record);
            }
        }
    }
}
