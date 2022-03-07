using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the remove command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the remove command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
        protected override void Handle(AppCommandRequest request)
        {
            this.Remove(request.Parameters);
        }

        private void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                throw new ArgumentException($"Cannot parse id \'{parameters}\'.");
            }

            bool deleted = this.Service.Remove(id);
            Console.WriteLine($"Record #{id} {(deleted ? "is removed" : "does not exists")}.");
        }
    }
}
