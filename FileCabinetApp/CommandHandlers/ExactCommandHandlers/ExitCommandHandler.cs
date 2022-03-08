using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the exit command.
    /// </summary>
    public class ExitCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<bool> setProgramRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Setvice to work with.</param>
        /// <param name="setProgramRunning">For stopping program.</param>
        public ExitCommandHandler(IFileCabinetService service, Action<bool> setProgramRunning)
            : base(service)
        {
            this.setProgramRunning = setProgramRunning;
        }

        /// <summary>
        /// Handles the exit command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
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
