using System.Collections;
using System.Collections.ObjectModel;

using Models;

namespace FileCabinetApp.Iterators
{
    public class FileSystemEnumerable : IEnumerable<FileCabinetRecord>
    {
        private readonly ReadOnlyCollection<long> positions;
        private readonly BinaryReader reader;
        private readonly Func<long, FileCabinetRecord> read;

        public FileSystemEnumerable(BinaryReader reader, ReadOnlyCollection<long> positions, Func<long, FileCabinetRecord> read)
        {
            this.reader = reader;
            this.positions = positions;
            this.read = read;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new FileSystemEnumerator(this.reader, this.positions, this.read);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FileSystemEnumerator(this.reader, this.positions, this.read);
        }
    }
}
