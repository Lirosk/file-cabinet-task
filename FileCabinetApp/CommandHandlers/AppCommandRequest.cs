namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents request for command with given parameters.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Gets requested command.
        /// </summary>
        /// <value>Requested command.</value>
        public string Command { get; init; } = string.Empty;

        /// <summary>
        /// Gets parameters for requested command.
        /// </summary>
        /// <value>Parameters for requested command.</value>
        public string Parameters { get; init; } = string.Empty;
    }
}
