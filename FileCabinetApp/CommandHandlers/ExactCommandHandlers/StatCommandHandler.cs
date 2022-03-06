using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service)
            : base("stat", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Stat(request.Parameters);
        }

        private static void Stat(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            var recordsCount = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.have} record(s) total.");
            Console.WriteLine($"{recordsCount.deleted} record(s) deleted.");
        }
    }
}
