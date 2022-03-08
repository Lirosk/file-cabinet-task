using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handles command which requires work with service.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.Service = service;
        }

        /// <summary>
        /// Gets service with which one this handler works.
        /// </summary>
        /// <value>Service with which one this handler works.</value>
        public IFileCabinetService Service { get; private set; }
    }
}
