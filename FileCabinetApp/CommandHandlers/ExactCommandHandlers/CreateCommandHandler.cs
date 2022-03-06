using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class CreateCommandHandler : CommandHandlerBase
    {
        public CreateCommandHandler(IFileCabinetService service)
            : base("create", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Create(request.Parameters);
        }

        private static void Create(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            while (Program.IsRunning)
            {
                try
                {
                    RecordHelper.ReadRecordDataFromConsole(FileCabinetRecord.InputDateTimeFormat, out var personalData);

                    Console.WriteLine(
                        "Record #{0} is created.",
                        Program.FileCabinetService.CreateRecord(personalData));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                    continue;
                }

                break;
            }
        }
    }
}
