﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using FileCabinetApp.Validators;
using Models;

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
        private readonly SortedDictionary<(string fieldName, string value), List<long>> index = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Stream to save records.</param>
        /// <param name="validator">Validator for checking records.</param>
        public FileCabinetFileSystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
            this.SetIndex();
        }

        /// <summary>
        /// Create record from given parameters.
        /// </summary>
        /// <param name="personalData">Represents data of a person.</param>
        /// <returns>Returns the id of created record.</returns>
        public int CreateRecord(PersonalData personalData)
        {
            this.validator.Validate(personalData);
            var binaryWriter = new BinaryWriter(this.fileStream, Program.EncodingUsed);

            var id = this.GetStat().have + 1;

            // after GetStat() fileStream's position in the end.
            this.AddToIndex(personalData, this.fileStream.Position);

            byte[] buffer = BitConverter.GetBytes((short)0);
            binaryWriter.Write(buffer);

            buffer = BitConverter.GetBytes(id);
            binaryWriter.Write(buffer);

            buffer = new byte[FirstNameSize];
            Program.EncodingUsed.GetBytes(personalData.FirstName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            buffer = new byte[LastNameSize];
            Program.EncodingUsed.GetBytes(personalData.LastName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            buffer = BitConverter.GetBytes(personalData.DateOfBirth.Year);
            binaryWriter.Write(buffer);

            buffer = BitConverter.GetBytes(personalData.DateOfBirth.Month);
            binaryWriter.Write(buffer);

            buffer = BitConverter.GetBytes(personalData.DateOfBirth.Day);
            binaryWriter.Write(buffer);

            buffer = BitConverter.GetBytes(personalData.SchoolGrade);
            binaryWriter.Write(buffer);

            foreach (var int32 in decimal.GetBits(personalData.AverageMark))
            {
                buffer = BitConverter.GetBytes(int32);
                binaryWriter.Write(buffer);
            }

            buffer = BitConverter.GetBytes(personalData.ClassLetter);
            binaryWriter.Write(buffer);

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

            this.RemoveFromIndex(offset);
            this.AddToIndex(newData, offset);

            var binaryWriter = new BinaryWriter(this.fileStream, Program.EncodingUsed);

            byte[] buffer;

            buffer = new byte[FirstNameSize];
            Program.EncodingUsed.GetBytes(newData.FirstName).CopyTo(buffer, 0);
            binaryWriter.Write(buffer);

            buffer = new byte[LastNameSize];
            Program.EncodingUsed.GetBytes(newData.LastName).CopyTo(buffer, 0);
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
        public IEnumerable<FileCabinetRecord> FindByField(string fieldName, string value)
        {
            var key = (fieldName.ToUpperInvariant(), value);

            var reader = new BinaryReader(this.fileStream);

            if (!this.index.TryGetValue(key, out var positions))
            {
                yield break;
            }

            foreach (var position in positions)
            {
                if (TryReadRecord(reader, position, out var record))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Get all stored records.
        /// </summary>
        /// <returns>All stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var list = new List<FileCabinetRecord>();
            var reader = new BinaryReader(this.fileStream, Program.EncodingUsed);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                if (TryReadRecord(reader, reader.BaseStream.Position, out var record))
                {
                    list.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Get count of stored records.
        /// </summary>
        /// <returns>Count of stored records.</returns>
        public (int have, int deleted) GetStat()
        {
            int have = (int)(this.fileStream.Length / RecordSize);
            int deleted = 0;

            this.fileStream.Seek(0, SeekOrigin.Begin);

            var binaryReader = new BinaryReader(this.fileStream, Program.EncodingUsed);

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                var status = binaryReader.ReadInt16();

                if (IsDeleted(status))
                {
                    deleted++;
                }

                this.fileStream.Position += RecordSize - StatusSize;
            }

            return (have, deleted);
        }

        /// <summary>
        /// Makes snapshot of present records.
        /// </summary>
        /// <returns>Snapshot of present records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
        }

        /// <summary>
        /// Restore records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot contatining records to restore.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            var haveRecordsWithIds = this.IdsOfStoredRecords();
            this.fileStream.Seek(0, SeekOrigin.End);
            byte[] buffer;
            int imported = 0;

            var binaryWriter = new BinaryWriter(this.fileStream, Program.EncodingUsed);

            foreach (var record in snapshot.Records)
            {
                try
                {
                    this.validator.Validate(new ()
                    {
                        FirstName = record.FirstName,
                        LastName = record.LastName,
                        DateOfBirth = record.DateOfBirth,
                        SchoolGrade = record.SchoolGrade,
                        AverageMark = record.AverageMark,
                        ClassLetter = record.ClassLetter,
                    });
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                if (!haveRecordsWithIds.Contains(record.Id))
                {
                    this.AddToIndex(record, this.fileStream.Position);

                    binaryWriter.Write((short)0);
                    binaryWriter.Write(record.Id);

                    buffer = new byte[FirstNameSize];
                    Program.EncodingUsed.GetBytes(record.FirstName).CopyTo(buffer, 0);
                    binaryWriter.Write(buffer);

                    buffer = new byte[LastNameSize];
                    Program.EncodingUsed.GetBytes(record.LastName).CopyTo(buffer, 0);
                    binaryWriter.Write(buffer);

                    binaryWriter.Write(record.DateOfBirth.Year);
                    binaryWriter.Write(record.DateOfBirth.Month);
                    binaryWriter.Write(record.DateOfBirth.Day);

                    binaryWriter.Write(record.SchoolGrade);
                    binaryWriter.Write(record.AverageMark);

                    binaryWriter.Write(record.ClassLetter);

                    imported++;
                }
            }

            Console.WriteLine($"{imported} record were imported.");
        }

        /// <inheritdoc/>
        public bool Remove(int recordId)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var binaryReader = new BinaryReader(this.fileStream, Program.EncodingUsed);
            var binaryWriter = new BinaryWriter(this.fileStream, Program.EncodingUsed);

            for (; this.fileStream.Position < this.fileStream.Length; this.fileStream.Position += RecordSize - StatusSize - IdSize)
            {
                var status = binaryReader.ReadInt16();
                var id = binaryReader.ReadInt32();

                if (id == recordId)
                {
                    binaryWriter.BaseStream.Position -= StatusSize + IdSize;

                    this.RemoveFromIndex(this.fileStream.Position);

                    binaryWriter.Write(status | (int)FileCabinetRecordStatus.Deleted);

                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public int Purge()
        {
            this.fileStream.Position = 0;

            var br = new BinaryReader(this.fileStream, Program.EncodingUsed);
            var bw = new BinaryWriter(this.fileStream, Program.EncodingUsed);

            var buffer = new byte[RecordSize];
            short status;
            long currentPosition;

            int purged = 0;

            while (br.Read(buffer, 0, RecordSize) == RecordSize)
            {
                status = BitConverter.ToInt16(buffer, 0);
                if (IsDeleted(status))
                {
                    currentPosition = this.fileStream.Position;

                    while (br.Read(buffer, 0, RecordSize) == RecordSize)
                    {
                        this.fileStream.Position -= 2 * RecordSize;
                        bw.Write(buffer);
                        this.fileStream.Position += RecordSize;
                    }

                    this.fileStream.Position = currentPosition;
                    this.fileStream.SetLength(this.fileStream.Length - RecordSize);

                    purged++;
                }
            }

            return purged;
        }

        private static bool TryReadRecord(BinaryReader reader, long position, out FileCabinetRecord record)
        {
            try
            {
                reader.BaseStream.Position = position;

                var status = reader.ReadInt16();
                if (IsDeleted(status))
                {
                    reader.BaseStream.Position += RecordSize - StatusSize;
                    record = new ();
                    return false;
                }

                var id = reader.ReadInt32();

                int index;

                var firstName = Program.EncodingUsed.GetString(reader.ReadBytes(FirstNameSize));
                if ((index = firstName.IndexOf((char)0, StringComparison.Ordinal)) != -1)
                {
                    firstName = firstName.Substring(0, index);
                }

                var lastName = Program.EncodingUsed.GetString(reader.ReadBytes(LastNameSize));
                if ((index = lastName.IndexOf((char)0, StringComparison.Ordinal)) != -1)
                {
                    lastName = lastName.Substring(0, index);
                }

                var year = reader.ReadInt32();
                var month = reader.ReadInt32();
                var day = reader.ReadInt32();

                var dateOfBirth = new DateTime(year, month, day);

                var schoolGrade = reader.ReadInt16();
                var averageMark = reader.ReadDecimal();
                var classLetter = reader.ReadChar();

                record = new ()
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    SchoolGrade = schoolGrade,
                    AverageMark = averageMark,
                    ClassLetter = classLetter,
                };
            }
            catch (Exception)
            {
                reader.BaseStream.Position = position + RecordSize;
                record = new ();
                return false;
            }

            return true;
        }

        private static bool IsDeleted(short status)
        {
            return (status & (short)FileCabinetRecordStatus.Deleted) > 0;
        }

        private int[] IdsOfStoredRecords()
        {
            var res = new List<int>();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            short status;

            var bytes = new byte[RecordSize];
            while (this.fileStream.Read(bytes, 0, RecordSize) == RecordSize)
            {
                status = BitConverter.ToInt16(bytes, 0);
                if (IsDeleted(status))
                {
                    continue;
                }

                res.Add(BitConverter.ToInt32(bytes, StatusSize));
            }

            return res.ToArray();
        }

        private int FindRecordOffset(int id)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            var binaryReader = new BinaryReader(this.fileStream, Program.EncodingUsed);

            var buffer = new byte[RecordSize];
            var offset = 0;
            int readedId;
            short status;

            for (; binaryReader.Read(buffer, 0, RecordSize) == RecordSize; offset += RecordSize)
            {
                status = BitConverter.ToInt16(buffer, 0);
                if (IsDeleted(status))
                {
                    continue;
                }

                readedId = BitConverter.ToInt32(buffer, StatusSize);
                if (readedId == id)
                {
                    return offset;
                }
            }

            throw new ArgumentException($"No record with {id} id.");
        }

        private void AddToIndex(FileCabinetRecord record, long position)
        {
            var properties = record.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string fieldName;
            object value;

            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = FileCabinetRecord.OutputDateTimeFormat;
            culture.DateTimeFormat.LongTimePattern = string.Empty;
            culture.NumberFormat.NumberDecimalSeparator = ".";

            foreach (var property in properties)
            {
                fieldName = property.Name;
                if (fieldName.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                value = property.GetValue(record) !;

                var key = (fieldName.ToUpperInvariant(), Convert.ToString(value, culture) !.Trim());

                if (this.index.ContainsKey(key))
                {
                    this.index[key].Add(position);
                }
                else
                {
                    this.index.Add(key, new List<long> { position });
                }
            }
        }

        private void AddToIndex(PersonalData data, long position)
        {
            this.AddToIndex(new FileCabinetRecord(-1, data), position);
        }

        private void RemoveFromIndex(long position)
        {
            foreach (var (key, positions) in this.index)
            {
                if (positions.Contains(position))
                {
                    positions.Remove(position);
                    if (positions.Count == 0)
                    {
                        this.index.Remove(key);
                    }
                }
            }
        }

        private void SetIndex()
        {
            this.fileStream.Position = 0;

            var buffer = new byte[RecordSize];

            var reader = new BinaryReader(this.fileStream, Program.EncodingUsed);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                if (TryReadRecord(reader, this.fileStream.Position, out var record))
                {
                    this.AddToIndex(
                        record,
                        this.fileStream.Position - RecordSize);
                }
            }
        }
    }
}
