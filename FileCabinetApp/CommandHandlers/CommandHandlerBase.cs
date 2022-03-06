using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        protected IFileCabinetService Service { get; private set; }

        private string commandName = string.Empty;
        private ICommandHandler? nextHandler;

        protected CommandHandlerBase(string commandName, IFileCabinetService service)
            : this(commandName)
        {
            this.Service = service;
        }

        protected CommandHandlerBase(string commandName)
        {
            this.commandName = commandName;
        }

        void ICommandHandler.Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals(this.commandName, StringComparison.InvariantCultureIgnoreCase))
            {
                _ = this.nextHandler ?? throw new ArgumentException("Invalid command to handle.");
                this.nextHandler.Handle(request);
                return;
            }

            this.Handle(request);
        }

        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }

        protected abstract void Handle(AppCommandRequest request);
    }
}
