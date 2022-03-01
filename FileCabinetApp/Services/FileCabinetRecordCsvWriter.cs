using System.Globalization;
using System.Reflection;
using System.Text;

using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class for writing data to csv file.
    /// </summary>
    public sealed class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
        private readonly TextWriter writer;
        private readonly StringBuilder sb = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Writer for saving data.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
            var properties = typeof(FileCabinetRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                this.sb.Append(property.Name);
                this.sb.Append(',');
            }

            this.sb.Remove(this.sb.Length - 1, 1);
            this.writer.WriteLine(this.sb);
            this.sb.Clear();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.writer.Flush();
            this.writer.Dispose();
        }

        /// <summary>
        /// Writes record to given stream.
        /// </summary>
        /// <param name="record">Record to write.</param>
        public void Write(FileCabinetRecord record)
        {
            var properties = typeof(FileCabinetRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            object value;

            var customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            foreach (var property in properties)
            {
                value = property.GetValue(record) !;
                if (value.GetType().Equals(typeof(DateTime)))
                {
                    this.sb.Append(((DateTime)value).ToString(FileCabinetRecord.InputDateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    this.sb.Append(Convert.ToString(value, customCulture));
                }

                this.sb.Append(',');
            }

            this.sb.Remove(this.sb.Length - 1, 1);
            this.writer.WriteLine(this.sb);
            this.sb.Clear();
        }
    }
}
