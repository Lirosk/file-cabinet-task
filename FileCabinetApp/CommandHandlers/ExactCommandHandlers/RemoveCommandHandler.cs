using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public RemoveCommandHandler(IFileCabinetService service)
            : base("remove", service)
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            this.Remove(request.Parameters);
        }

        private void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                throw new ArgumentException($"Cannot parse id \'{parameters}\'.");
            }

            bool deleted = this.Service.Remove(id);
            Console.WriteLine($"Record #{id} {(deleted ? "is removed" : "does not exists")}.");
        }
    }
}
