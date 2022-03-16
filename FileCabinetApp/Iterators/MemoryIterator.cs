using System.Collections;
using System.Collections.ObjectModel;

using Models;

namespace FileCabinetApp.Iterators
{
    public sealed class MemoryIterator : IRecordIterator
    {
        private ReadOnlyCollection<FileCabinetRecord> records;
        private int position;

        public MemoryIterator(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

        public FileCabinetRecord GetNext()
        {
            return this.records[this.position++];
        }

        public bool HasNext()
        {
            return this.position < this.records.Count;
        }
    }
}
