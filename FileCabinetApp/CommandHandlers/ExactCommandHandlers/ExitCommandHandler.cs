using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ExitCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<bool> setProgramRunning;

        public ExitCommandHandler(IFileCabinetService service, Action<bool> setProgramRunning)
            : base("exit", service)
        {
            this.setProgramRunning = setProgramRunning;
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Exit(request.Parameters);
        }

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            this.setProgramRunning(false);
        }
    }
}
