using FileCabinetApp.Services;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ExportCommandHandler : CommandHandlerBase
    {
        public ExportCommandHandler(IFileCabinetService service)
            : base("export", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Export(request.Parameters);
        }

        private static void Export(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            var spaceIndex = parameters.IndexOf(' ', StringComparison.Ordinal);

            if (spaceIndex == -1)
            {
                throw new ArgumentException("Invalid export parameters");
            }

            var extension = parameters[..spaceIndex];
            var filePath = parameters[(spaceIndex + 1)..];

            using var writer = new StreamWriter(filePath, false, Program.EncodingUsed);
            var snapshot = Program.FileCabinetService.MakeSnapshot();

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
