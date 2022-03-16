using System.Collections.ObjectModel;

using Models;

namespace FileCabinetApp.Iterators
{
    public class FileSystemIterator : IRecordIterator
    {
        private readonly ReadOnlyCollection<long> positions;
        private readonly BinaryReader reader;
        private readonly Func<long, FileCabinetRecord> read;
        private int currentPositionIndex;

        public FileSystemIterator(BinaryReader reader, ReadOnlyCollection<long> positions, Func<long, FileCabinetRecord> read)
        {
            this.reader = reader;
            this.positions = positions;
            this.read = read;
        }

        public FileCabinetRecord GetNext()
        {
            return this.read(this.positions[this.currentPositionIndex++]);
        }

        public bool HasNext()
        {
            return this.currentPositionIndex < this.positions.Count;
        }
    }
}
