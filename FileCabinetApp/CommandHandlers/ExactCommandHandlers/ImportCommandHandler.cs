using FileCabinetApp.Services;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ImportCommandHandler : CommandHandlerBase
    {
        public ImportCommandHandler(IFileCabinetService service)
            : base("import", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Import(request.Parameters);
        }

        private static void Import(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

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
                        ImportCsv(filePath);
                        break;
                    }

                case "xml":
                    {
                        ImportXml(filePath);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException($"Extension {extension} is unsupportable.");
                    }
            }
        }

        private static void ImportCsv(string filePath)
        {
            using var reader = new StreamReader(filePath, Program.EncodingUsed);
            var snapshot = new FileCabinetServiceSnapshot();
            snapshot.LoadFromCsv(reader);
            Program.FileCabinetService!.Restore(snapshot);
        }

        private static void ImportXml(string filePath)
        {
            using var reader = new StreamReader(filePath, Program.EncodingUsed);
            var snapshot = new FileCabinetServiceSnapshot();
            snapshot.LoadFromXml(reader);
            Program.FileCabinetService!.Restore(snapshot);
        }
    }
}
