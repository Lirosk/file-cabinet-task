using Models;

namespace FileCabinetApp.RecordPrinters
{
    public interface IRecordPrinter
    {
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}
