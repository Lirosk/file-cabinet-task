using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class for writing data to csv file.
    /// </summary>
    internal class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;
        private StringBuilder sb = new ();

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

            this.writer.Flush();
        }

        /// <summary>
        /// Writes record to <see cref="FileCabinetRecordCsvWriter.writer"/>.
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

            this.writer.Flush();
        }
    }
}
