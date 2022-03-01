using System.Globalization;
using System.Xml;

using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class for writing data to xml file.
    /// </summary>
    public sealed class FileCabinetRecordXmlWriter : IFileCabinetRecordWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Writer for saving data.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            this.writer = XmlWriter.Create(writer, settings);
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("records");
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();
            this.writer.Flush();
            this.writer.Dispose();
        }

        /// <summary>
        /// Writes record to given stream.
        /// </summary>
        /// <param name="record">Record to write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteStartElement("record");

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", record.FirstName);
            this.writer.WriteAttributeString("last", record.LastName);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("dateOfBirth");
            this.writer.WriteString(record.DateOfBirth.ToString(FileCabinetRecord.InputDateTimeFormat, CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("schoolGrade");
            this.writer.WriteValue(record.SchoolGrade);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("averageMark");
            this.writer.WriteValue(record.AverageMark);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("classLetter");
            this.writer.WriteString(record.ClassLetter.ToString());
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
        }
    }
}