using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        void ICommandHandler.Handle(AppCommandRequest request)
        {
            if (!this.GetType().Name.StartsWith(request.Command, StringComparison.InvariantCultureIgnoreCase))
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
