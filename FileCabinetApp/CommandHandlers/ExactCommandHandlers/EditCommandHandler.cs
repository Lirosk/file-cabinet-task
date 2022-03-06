using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class EditCommandHandler : CommandHandlerBase
    {
        public EditCommandHandler(IFileCabinetService service)
            : base("edit", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Edit(request.Parameters);
        }

        private static void Edit(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            int id;

            while (Program.IsRunning)
            {
                try
                {
                    Console.Write("id: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        throw new ArgumentException("Invalid input for id.");
                    }

                    RecordHelper.ReadRecordDataFromConsole(FileCabinetRecord.OutputDateTimeFormat, out var personalData);

                    Program.FileCabinetService.EditRecord(id, personalData);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                    continue;
                }

                Console.WriteLine($"Record #{id} is updated.");
                break;
            }
        }
    }
}
