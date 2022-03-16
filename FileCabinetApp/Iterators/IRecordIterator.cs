using Models;

namespace FileCabinetApp.Iterators
{
    public interface IRecordIterator
    {
        FileCabinetRecord GetNext();

        bool HasNext();
    }
}
