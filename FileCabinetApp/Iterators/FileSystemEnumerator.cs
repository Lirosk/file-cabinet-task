using System.Collections;
using System.Collections.ObjectModel;
using Models;

namespace FileCabinetApp.Iterators
{
    public class FileSystemEnumerator : IEnumerator<FileCabinetRecord>
    {
        private readonly ReadOnlyCollection<long> positions;
        private readonly BinaryReader reader;
        private readonly Func<long, FileCabinetRecord> read;
        private int currentPositionIndex = -1;

        public FileSystemEnumerator(BinaryReader reader, ReadOnlyCollection<long> positions, Func<long, FileCabinetRecord> read)
        {
            this.reader = reader;
            this.positions = positions;
            this.read = read;
        }

        public FileCabinetRecord Current
        {
            get
            {
                return this.read(this.positions[this.currentPositionIndex]);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return ++this.currentPositionIndex < this.positions.Count;
        }

        public void Reset()
        {
            this.currentPositionIndex = -1;
        }
    }
}
