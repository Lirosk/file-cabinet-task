namespace FileCabinetApp.Services
{
    /// <summary>
    /// Represents status of record for FileCabinetFileSystemService.
    /// </summary>
    [Flags]
    public enum FileCabinetRecordStatus
    {
        /// <summary>
        /// Status of deleted record.
        /// </summary>
        Deleted = 1 << 2,
    }
}
