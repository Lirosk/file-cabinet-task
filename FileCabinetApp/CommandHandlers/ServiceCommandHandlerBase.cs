using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        public IFileCabinetService Service { get; private set; }

        public ServiceCommandHandlerBase(string commandName, IFileCabinetService service)
            : base(commandName)

        {
            this.Service = service;
        }
    }
}
