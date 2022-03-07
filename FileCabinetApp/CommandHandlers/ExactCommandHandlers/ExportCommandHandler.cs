using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the export command.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the exit command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
        protected override void Handle(AppCommandRequest request)
        {
            this.Export(request.Parameters);
        }

        private void Export(string parameters)
        {
            var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

            if (spaceIndex == -1)
            {
                throw new ArgumentException("Invalid export parameters");
            }

            var extension = parameters[..spaceIndex];
            var filePath = parameters[(spaceIndex + 1) ..];

            using var writer = new StreamWriter(filePath, false, Program.EncodingUsed);
            var snapshot = this.Service.MakeSnapshot();

            switch (extension)
            {
                case "csv":
                    {
                        snapshot.SaveToCsv(writer);
                        break;
                    }

                case "xml":
                    {
                        snapshot.SaveToXml(writer);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException($"Extension {extension} is unsupportable.");
                    }
            }
        }
    }
}
