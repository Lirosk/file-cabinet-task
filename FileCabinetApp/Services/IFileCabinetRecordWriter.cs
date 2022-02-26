namespace FileCabinetApp.Services
{
    /// <summary>
    /// Interface for writing data to file.
    /// </summary>
    public interface IFileCabinetRecordWriter : IDisposable
    {
        /// <summary>
        /// Writes record to given stream.
        /// </summary>
        /// <param name="record">Record to write.</param>
        void Write(FileCabinetRecord record);
    }
}
