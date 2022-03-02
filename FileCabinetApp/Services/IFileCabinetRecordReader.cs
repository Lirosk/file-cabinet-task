using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Interface for reading records from stream.
    /// </summary>
    public interface IFileCabinetRecordReader
    {
        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>All readed records.</returns>
        public IList<FileCabinetRecord> ReadAll();
    }
}
