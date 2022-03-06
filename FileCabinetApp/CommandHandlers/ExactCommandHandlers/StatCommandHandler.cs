using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service)
            : base("stat", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Stat(request.Parameters);
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.Service.GetStat();
            Console.WriteLine($"{recordsCount.have} record(s) total.");
            Console.WriteLine($"{recordsCount.deleted} record(s) deleted.");
        }
    }
}
