namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Interface for command handle.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next handler if current is unsuitable.
        /// </summary>
        /// <param name="handler">Handler to set.</param>
        void SetNext(ICommandHandler handler);

        /// <summary>
        /// Handles the command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
        void Handle(AppCommandRequest request);
    }
}
