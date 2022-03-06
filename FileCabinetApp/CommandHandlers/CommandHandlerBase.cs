namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public void Handle(AppCommandRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
