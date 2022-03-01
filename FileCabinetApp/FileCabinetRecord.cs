using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents the record with personal information.
    /// </summary>
    [XmlRoot("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="personalData">Personal information.</param>
        public FileCabinetRecord(int id, PersonalData personalData)
        {
            this.Id = id;
            this.FirstName = personalData.FirstName;
            this.LastName = personalData.LastName;
            this.DateOfBirth = personalData.DateOfBirth;
            this.SchoolGrade = personalData.SchoolGrade;
            this.AverageMark = personalData.AverageMark;
            this.ClassLetter = personalData.ClassLetter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {
        }

        /// <summary>
        /// Gets or sets <see cref="DateTime">DateTime</see> format for record input.
        /// </summary>
        /// <value><see cref="DateTime">DateTime</see> format for record input.</value>
        public static string InputDateTimeFormat { get; protected set; } = "MM/dd/yyyy";

        /// <summary>
        /// Gets or sets <see cref="DateTime">DateTime</see> format for record output.
        /// </summary>
        /// <value><see cref="DateTime">DateTime</see> format for record output.</value>
        public static string OutputDateTimeFormat { get; protected set; } = "yyyy-MMM-dd";

        /// <summary>
        /// Gets or sets id of record.
        /// </summary>
        /// <value>Id of record.</value>
        [XmlAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of a person in record.
        /// </summary>
        /// <value>First name of a person in record.</value>
        [XmlElement]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets last name of a person in record.
        /// </summary>
        /// <value>Last name of a person in record.</value>
        [XmlElement]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth of a person in record.
        /// </summary>
        /// <value>Date of birth of a person in record.</value>
        [XmlElement]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets school grade of a person in record.
        /// </summary>
        /// <value>School grade of a person in record.</value>
        [XmlElement]
        public short SchoolGrade { get; set; }

        /// <summary>
        /// Gets or sets averange mark of a person in record.
        /// </summary>
        /// <value>Average mark of a person in record.</value>
        [XmlElement]
        public decimal AverageMark { get; set; }

        /// <summary>
        /// Gets or sets class letter of a person in record.
        /// </summary>
        /// <value>Class letter of a person in record.</value>
        [XmlElement]
        public char ClassLetter { get; set; }

        /// <summary>
        /// Convert instance to its string representation.
        /// </summary>
        /// <returns>Instance string representation.</returns>
        public override string ToString()
        {
            return
                $"#{this.Id}, {this.FirstName}, {this.LastName}, " +
                $"{this.DateOfBirth.ToString(FileCabinetRecord.OutputDateTimeFormat, CultureInfo.InvariantCulture)}, " +
                $"{this.SchoolGrade}, {this.AverageMark}, {this.ClassLetter}";
        }
    }
}
