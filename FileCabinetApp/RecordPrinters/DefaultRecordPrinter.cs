using Models;

namespace FileCabinetApp.RecordPrinters
{
    public class DefaultRecordPrinter : IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }
    }
}
