using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        public IFileCabinetService Service { get; private set; }

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.Service = service;
        }
    }
}
