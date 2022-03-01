using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Represents snapshot of once real records.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Gets readonly collection of records.
        /// </summary>
        /// <value>Readonly collection of records.</value>
        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.records);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Records to save.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Saves record to csv file.
        /// </summary>
        /// <param name="writer">Writer for writing records to it.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            using var scvWriter = new FileCabinetRecordCsvWriter(writer);
            foreach (var record in this.records)
            {
                scvWriter.Write(record);
            }
        }

        /// <summary>
        /// Saves record to xml file.
        /// </summary>
        /// <param name="writer">Writer for writing records to it.</param>
        public void SaveToXml(StreamWriter writer)
        {
            using var scvWriter = new FileCabinetRecordXmlWriter(writer);
            foreach (var record in this.records)
            {
                scvWriter.Write(record);
            }
        }

        /// <summary>
        /// Loads records from csv file.
        /// </summary>
        /// <param name="reader">Stream for reading.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            using var csvReader = new FileCabinetRecordCsvReader(reader);
            this.records = csvReader.ReadAll().ToArray();
        }
    }
}