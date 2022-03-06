using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base("purge", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Purge(request.Parameters);
        }

        private static void Purge(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            int was = Program.FileCabinetService.GetStat().have;
            int deleted = Program.FileCabinetService.Purge();
            Console.WriteLine($"{deleted} of {was} records were purged.");
        }
    }
}
