using System.Xml;
using System.Xml.Serialization;

using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Reads records from xml file.
    /// </summary>
    public class FileCabinetRecordXmlReader : IFileCabinetRecordReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">Reader to read data from.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>All readed records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var serializer = new XmlSerializer(typeof(List<RecordForXml>), null, null, new ("records"), null);
            var xmlReader = new XmlTextReader(this.reader);

            var res = (serializer.Deserialize(xmlReader) as List<RecordForXml>)?.Select(i =>
            {
                i.FirstName = i.FullName.FirstName!;
                i.LastName = i.FullName.LastName!;
                return i as FileCabinetRecord;
            }).ToList<FileCabinetRecord>();

            return res!;
        }
    }
}
