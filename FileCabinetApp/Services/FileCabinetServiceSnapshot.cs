using System.Collections.ObjectModel;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Represents snapshot of once real records.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Records to save.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Saves record to csv file.
        /// </summary>
        /// <param name="writer">Writer for writing records to it.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var scvWriter = new FileCabinetRecordCsvWriter(writer);
            foreach (var record in this.records)
            {
                scvWriter.Write(record);
            }
        }
    }
}