namespace FileCabinetApp.CommandHandlers.ExactCommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        public ExitCommandHandler()
            : base("exit")
        {
        }

        protected override void Handle(AppCommandRequest request)
        {
            Exit(request.Parameters);
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }
    }
}
