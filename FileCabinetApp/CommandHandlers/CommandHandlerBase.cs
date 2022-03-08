namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Abstract hadnler for command.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Handles the command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
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

        /// <summary>
        /// Sets next handler if current is unsuitable.
        /// </summary>
        /// <param name="handler">Handler to set.</param>
        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }

        /// <summary>
        /// Handles the command in its own way.
        /// </summary>
        /// <param name="request">Request for command to handle.</param>
        protected abstract void Handle(AppCommandRequest request);
    }
}
