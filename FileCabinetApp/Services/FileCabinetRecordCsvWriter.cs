using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class for writing data to csv file.
    /// </summary>
    public sealed class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
#pragma warning disable CA2213 // Следует высвобождать высвобождаемые поля
        private readonly TextWriter writer;
#pragma warning restore CA2213 // Следует высвобождать высвобождаемые поля
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
        }

        /// <summary>
        /// Writes record to given stream.
        /// </summary>
        /// <param name="record">Record to write.</param>
        public void Write(FileCabinetRecord record)
        {
            var properties = typeof(FileCabinetRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            object value;

            foreach (var property in properties)
            {
                value = property.GetValue(record) !;
                if (value.GetType().Equals(typeof(DateTime)))
                {
                    this.sb.Append(((DateTime)value).ToString(FileCabinetRecord.InputDateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    this.sb.Append(value);
                }

                this.sb.Append(',');
            }

            this.sb.Remove(this.sb.Length - 1, 1);
            this.writer.WriteLine(this.sb);
            this.sb.Clear();
        }
    }
}
