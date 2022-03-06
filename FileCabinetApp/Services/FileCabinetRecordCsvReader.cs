using System.Globalization;
using System.Reflection;

using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Reads records from csv file.
    /// </summary>
    public sealed class FileCabinetRecordCsvReader : IFileCabinetRecordReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">Reader to read data from.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>All readed records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> restored = new ();

            this.reader.BaseStream.Seek(0, SeekOrigin.Begin);

            var propertiesNames = this.reader.ReadLine() !.Split(',');
            var properties = propertiesNames.Select(
                name => typeof(FileCabinetRecord).GetProperty(
                    name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)).ToArray<PropertyInfo?>();

            string? readed;
            string[] readedValues;
            while ((readed = this.reader.ReadLine()) != null)
            {
                readedValues = readed.Split(',');

                if (readedValues.Length != properties.Length)
                {
                    continue;
                }

                var record = new FileCabinetRecord();

                for (int i = 0; i < readedValues.Length; i++)
                {
                    properties[i] !.SetValue(
                        record,
                        Convert.ChangeType(readedValues[i], properties[i] !.PropertyType, CultureInfo.InvariantCulture));
                }

                restored.Add(record);
            }

            return restored;
        }
    }
}
