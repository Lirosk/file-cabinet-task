namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        void SetNext(ICommandHandler handler);

        void Handle(AppCommandRequest request);
    }
}
