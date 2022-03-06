using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Purge(request.Parameters);
        }

        private void Purge(string parameters)
        {
            int was = this.Service.GetStat().have;
            int deleted = this.Service.Purge();
            Console.WriteLine($"{deleted} of {was} records were purged.");
        }
    }
}
