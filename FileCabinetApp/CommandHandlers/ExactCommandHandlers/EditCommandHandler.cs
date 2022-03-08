using FileCabinetApp.Helpers;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    /// <summary>
    /// Handles the edit command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public EditCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the edit command request.
        /// </summary>
        /// /// <param name="request">Request with data to handle.</param>
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
