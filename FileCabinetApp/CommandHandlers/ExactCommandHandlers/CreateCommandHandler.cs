using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public CreateCommandHandler(IFileCabinetService service)
            : base("create", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Create(request.Parameters);
        }

        private void Create(string parameters)
        {
            while (true)
            {
                try
                {
                    RecordHelper.ReadRecordDataFromConsole(FileCabinetRecord.InputDateTimeFormat, out var personalData);

                    Console.WriteLine(
                        "Record #{0} is created.",
                        this.Service.CreateRecord(personalData));
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
