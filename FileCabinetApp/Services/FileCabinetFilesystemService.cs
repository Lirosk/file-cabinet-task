﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
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
            byte[] buffer;

            binaryWriter.Write((short)0);
            binaryWriter.Write(id);

            buffer = new byte[FirstNameSize];
            Encoding.UTF8.GetBytes(personalData.FirstName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            buffer = new byte[LastNameSize];
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
            var offset = this.FindRecordOffset(id);

            this.fileStream.Seek(offset + StatusSize + IdSize, SeekOrigin.Begin);
            using var binaryWriter = new BinaryWriter(this.fileStream);

            byte[] buffer;

            buffer = new byte[FirstNameSize];
            Encoding.UTF8.GetBytes(newData.FirstName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            buffer = new byte[LastNameSize];
            Encoding.UTF8.GetBytes(newData.LastName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            binaryWriter.Write(newData.DateOfBirth.Year);
            binaryWriter.Write(newData.DateOfBirth.Month);
            binaryWriter.Write(newData.DateOfBirth.Day);

            binaryWriter.Write(newData.SchoolGrade);
            binaryWriter.Write(newData.AverageMark);
            binaryWriter.Write(newData.ClassLetter);

            binaryWriter.Flush();
        }

        /// <summary>
        /// Find record by value of given field.
        /// </summary>
        /// <param name="fieldName">Name of field to search.</param>
        /// <param name="value">Value of <paramref name="fieldName"/> field to search.</param>
        /// <returns>Array of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByField(string fieldName, object value)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            using var binaryReader = new BinaryReader(this.fileStream);

            var property = typeof(FileCabinetRecord).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property is null)
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }

            var buffer = new byte[RecordSize];

            var list = new List<FileCabinetRecord>();

            while (binaryReader.Read(buffer, 0, RecordSize) == RecordSize)
            {
                var record = RecordFromBytes(buffer);
                var recordFieldValue = property.GetValue(record);

                if (value.Equals(recordFieldValue))
                {
                    list.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(list);
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

            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                _ = binaryReader.ReadInt16();
                var id = binaryReader.ReadInt32();

                int index;

                var firstName = Encoding.UTF8.GetString(binaryReader.ReadBytes(FirstNameSize));
                if ((index = firstName.IndexOf((char)0, StringComparison.Ordinal)) != -1)
                {
                    firstName = firstName.Substring(0, index);
                }

                var lastName = Encoding.UTF8.GetString(binaryReader.ReadBytes(LastNameSize));
                if ((index = lastName.IndexOf((char)0, StringComparison.Ordinal)) != -1)
                {
                    lastName = lastName.Substring(0, index);
                }

                var year = binaryReader.ReadInt32();
                var month = binaryReader.ReadInt32();
                var day = binaryReader.ReadInt32();

                var dateOfBirth = new DateTime(year, month, day);

                var schoolGrade = binaryReader.ReadInt16();
                var averageMark = binaryReader.ReadDecimal();
                var classLetter = binaryReader.ReadChar();

                var personalData = new PersonalData()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    SchoolGrade = schoolGrade,
                    AverageMark = averageMark,
                    ClassLetter = classLetter,
                };

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

        private static FileCabinetRecord RecordFromBytes(byte[] bytes)
        {
            int offset = StatusSize;
            var id = BitConverter.ToInt32(bytes, offset);

            int index;

            offset += IdSize;
            var firstName = Encoding.UTF8.GetString(bytes, offset, FirstNameSize);
            if ((index = firstName.IndexOf((char)0, StringComparison.Ordinal)) != -1)
            {
                firstName = firstName.Substring(0, index);
            }

            offset += FirstNameSize;
            var lastName = Encoding.UTF8.GetString(bytes, offset, LastNameSize);
            if ((index = lastName.IndexOf((char)0, StringComparison.Ordinal)) != -1)
            {
                lastName = lastName.Substring(0, index);
            }

            offset += LastNameSize;
            var year = BitConverter.ToInt32(bytes, offset);

            offset += YearSize;
            var month = BitConverter.ToInt32(bytes, offset);

            offset += MonthSize;
            var day = BitConverter.ToInt32(bytes, offset);

            var dateOfBirth = new DateTime(year, month, day);

            offset += DaySize;
            var schoolGrade = BitConverter.ToInt16(bytes, offset);

            offset += schoolGrade;
            var i1 = BitConverter.ToInt32(bytes, offset);
            offset += AverageMarkSize / sizeof(int);
            var i2 = BitConverter.ToInt32(bytes, offset);
            offset += AverageMarkSize / sizeof(int);
            var i3 = BitConverter.ToInt32(bytes, offset);
            offset += AverageMarkSize / sizeof(int);
            var i4 = BitConverter.ToInt32(bytes, offset);
            offset += AverageMarkSize / sizeof(int);

            var averageMark = new decimal(new[] { i1, i2, i3, i4 });

            var classLetter = BitConverter.ToChar(bytes, offset);

            var personalData = new PersonalData()
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                SchoolGrade = schoolGrade,
                AverageMark = averageMark,
                ClassLetter = classLetter,
            };

            return new FileCabinetRecord(id, personalData);
        }

        private int FindRecordOffset(int id)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            using var binaryReader = new BinaryReader(this.fileStream);

            var buffer = new byte[RecordSize];
            var offset = 0;
            int readedId;

            while (binaryReader.Read(buffer, 0, RecordSize) == RecordSize)
            {
                readedId = BitConverter.ToInt32(buffer, StatusSize);
                if (readedId == id)
                {
                    return offset;
                }

                offset += RecordSize;
            }

            throw new ArgumentException($"No record with {id} id.");
        }
    }
}
