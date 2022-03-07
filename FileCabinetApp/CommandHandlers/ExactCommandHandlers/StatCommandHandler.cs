using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the stat command.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the stat command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
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
