using System.Xml.Serialization;

namespace Models
{
    /// <summary>
    /// For serializing it right way.
    /// </summary>
    [XmlType("record")]
    public class RecordForXml : FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordForXml"/> class.
        /// </summary>
        public RecordForXml()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordForXml"/> class.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="personalData">Personal information.</param>
        public RecordForXml(int id, PersonalData personalData)
            : base(id, personalData)
        {
            this.FullName.FirstName = personalData.FirstName;
            this.FullName.LastName = personalData.LastName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordForXml"/> class.
        /// </summary>
        /// <param name="record">Record for copying data.</param>
        public RecordForXml(FileCabinetRecord record)
        {
            this.Id = record.Id;
            this.FullName.FirstName = record.FirstName;
            this.FullName.LastName = record.LastName;
            this.DateOfBirth = record.DateOfBirth;
            this.SchoolGrade = record.SchoolGrade;
            this.AverageMark = record.AverageMark;
            this.ClassLetter = record.ClassLetter;
        }

        /// <summary>
        /// Gets or sets represent first and last names.
        /// </summary>
        /// <value>Object represent first and last names.</value>
        [XmlElement("name")]
        public Name FullName { get; set; } = new ();

        /// <summary>
        /// Represents first and last names.
        /// </summary>
        public class Name
        {
            /// <summary>
            /// Gets or sets first name of a person in record.
            /// </summary>
            /// <value>First name of a person in record.</value>
            [XmlAttribute]
            public string? FirstName { get; set; }

            /// <summary>
            /// Gets or sets last name of a person in record.
            /// </summary>
            /// <value>Last name of a person in record.</value>
            [XmlAttribute]
            public string? LastName { get; set; }
        }
    }
}
