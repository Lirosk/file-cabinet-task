using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        public EditCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Edit(request.Parameters);
        }

        private void Edit(string parameters)
        {
            int id;

            while (true)
            {
                try
                {
                    Console.Write("id: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        throw new ArgumentException("Invalid input for id.");
                    }

                    RecordHelper.ReadRecordDataFromConsole(FileCabinetRecord.OutputDateTimeFormat, out var personalData);

                    this.Service.EditRecord(id, personalData);
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
