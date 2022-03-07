using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the import command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the import command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
        protected override void Handle(AppCommandRequest request)
        {
            this.Import(request.Parameters);
        }

        private void Import(string parameters)
        {
            var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

            if (spaceIndex == -1)
            {
                throw new ArgumentException("Invalid export parameters");
            }

            var extension = parameters[..spaceIndex];
            var filePath = parameters[(spaceIndex + 1) ..];

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File does not exist.");
            }

            using var writer = new StreamReader(filePath, Program.EncodingUsed);

            switch (extension)
            {
                case "csv":
                    {
                        this.ImportCsv(filePath);
                        break;
                    }

                case "xml":
                    {
                        this.ImportXml(filePath);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException($"Extension {extension} is unsupportable.");
                    }
            }
        }

        private void ImportCsv(string filePath)
        {
            using var reader = new StreamReader(filePath, Program.EncodingUsed);
            var snapshot = new FileCabinetServiceSnapshot();
            snapshot.LoadFromCsv(reader);
            this.Service.Restore(snapshot);
        }

        private void ImportXml(string filePath)
        {
            using var reader = new StreamReader(filePath, Program.EncodingUsed);
            var snapshot = new FileCabinetServiceSnapshot();
            snapshot.LoadFromXml(reader);
            this.Service.Restore(snapshot);
        }
    }
}
