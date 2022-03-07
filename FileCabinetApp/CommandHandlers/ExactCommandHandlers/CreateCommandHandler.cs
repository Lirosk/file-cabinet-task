using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the create command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the create command request.
        /// </summary>
        /// <param name="request">Request with data to handle.</param>
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
