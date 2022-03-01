using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Interface for reading records from stream.
    /// </summary>
    public abstract class FileCabinetRecordReader
    {
        /// <summary>
        /// Read data from.
        /// </summary>
        protected StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordReader"/> class.
        /// </summary>
        /// <param name="reader">Reader to read data from.</param>
        protected FileCabinetRecordReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>All readed records.</returns>
        public abstract IList<FileCabinetRecord> ReadAll();
    }
}
