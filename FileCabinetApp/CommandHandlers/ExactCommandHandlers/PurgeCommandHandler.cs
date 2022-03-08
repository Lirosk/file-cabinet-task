using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the purge command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the purge command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
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
