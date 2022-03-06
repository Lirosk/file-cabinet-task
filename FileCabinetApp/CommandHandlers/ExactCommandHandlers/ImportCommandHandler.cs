using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        public ImportCommandHandler(IFileCabinetService service)
            : base("import", service)
        {
        }

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
            var filePath = parameters[(spaceIndex + 1)..];

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
