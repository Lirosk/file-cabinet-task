namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class RemoveCommandHandler : CommandHandlerBase
    {
        public RemoveCommandHandler()
            : base("remove")
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Remove(request.Parameters);
        }

        private static void Remove(string parameters)
        {
            _ = Program.FileCabinetService ?? throw new InvalidOperationException("No service set for Program.");

            if (!int.TryParse(parameters, out var id))
            {
                throw new ArgumentException($"Cannot parse id \'{parameters}\'.");
            }

            bool deleted = Program.FileCabinetService.Remove(id);
            Console.WriteLine($"Record #{id} {(deleted ? "is removed" : "does not exists")}.");
        }
    }
}
