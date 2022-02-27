using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Stores records with personal information; manages the creation, editing, finding the records.
    /// </summary>
    internal class FileCabinetFileSystemService : IFileCabinetService
    {
        private const byte StatusSize = sizeof(short);
        private const byte IdSize = sizeof(int);
        private const byte FirstNameSize = 120;
        private const byte LastNameSize = 120;
        private const byte YearSize = sizeof(int);
        private const byte MonthSize = sizeof(int);
        private const byte DaySize = sizeof(int);
        private const byte SchoolGradeSize = sizeof(short);
        private const byte AverageMarkSize = sizeof(decimal);
        private const byte ClassLetterSize = sizeof(char);
        private const int RecordSize =
            StatusSize + IdSize +
            FirstNameSize + LastNameSize +
            YearSize + MonthSize + DaySize +
            SchoolGradeSize + AverageMarkSize + ClassLetterSize;

        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Stream to save records.</param>
        /// <param name="validator">Validator for checking records.</param>
        public FileCabinetFileSystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        /// <summary>
        /// Create record from given parameters.
        /// </summary>
        /// <param name="personalData">Represents data of a person.</param>
        /// <returns>Returns the id of created record.</returns>
        public int CreateRecord(PersonalData personalData)
        {
            this.validator.ValidateParameters(personalData);
            using var binaryWriter = new BinaryWriter(this.fileStream);

            var id = this.GetStat() + 1;
            var buffer = new byte[120];

            binaryWriter.Write((short)0);
            binaryWriter.Write(id);

            Encoding.UTF8.GetBytes(personalData.FirstName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }

            Encoding.UTF8.GetBytes(personalData.LastName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            binaryWriter.Write(personalData.DateOfBirth.Year);
            binaryWriter.Write(personalData.DateOfBirth.Month);
            binaryWriter.Write(personalData.DateOfBirth.Day);
            binaryWriter.Write(personalData.SchoolGrade);
            binaryWriter.Write(personalData.AverageMark);
            binaryWriter.Write(personalData.ClassLetter);

            binaryWriter.Flush();

            return id;
        }

        /// <summary>
        /// Edit record with the given id.
        /// </summary>
        /// <param name="id">Id of record to edit.</param>
        /// <param name="newData">New personal data for record.</param>
        /// <exception cref="ArgumentException">No record matching given id.</exception>
        public void EditRecord(int id, PersonalData newData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find record by value of given field.
        /// </summary>
        /// <param name="fieldName">Name of field to search.</param>
        /// <param name="value">Value of <paramref name="fieldName"/> field to search.</param>
        /// <returns>Array of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByField(string fieldName, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all stored records.
        /// </summary>
        /// <returns>All stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var list = new List<FileCabinetRecord>();
            using var binaryReader = new BinaryReader(this.fileStream);

            var buffer = new byte[RecordSize];
            int offset;

            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                _ = binaryReader.ReadInt16();
                var id = binaryReader.ReadInt32();

                var personalData = new PersonalData();

                personalData.FirstName = Encoding.UTF8.GetString(binaryReader.ReadBytes(FirstNameSize));
                personalData.LastName = Encoding.UTF8.GetString(binaryReader.ReadBytes(LastNameSize));

                var year = binaryReader.ReadInt32();
                var month = binaryReader.ReadInt32();
                var day = binaryReader.ReadInt32();

                personalData.DateOfBirth = new (year, month, day);

                personalData.SchoolGrade = binaryReader.ReadInt16();
                personalData.AverageMark = binaryReader.ReadDecimal();
                personalData.ClassLetter = binaryReader.ReadChar();

                list.Add(new (id, personalData));
            }

            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Get count of stored records.
        /// </summary>
        /// <returns>Count of stored records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Seek(0, SeekOrigin.End) / RecordSize);
        }

        /// <summary>
        /// Makes snapshot of present records.
        /// </summary>
        /// <returns>Snapshot of present records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
