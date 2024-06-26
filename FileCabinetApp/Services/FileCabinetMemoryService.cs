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
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<(string, string), List<FileCabinetRecord>> index = new ();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">Validates personal data for record.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Makes snapshot of present records.
        /// </summary>
        /// <returns>Snapshot of present records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <summary>
        /// Create record from given parameters.
        /// </summary>
        /// <param name="personalData">Represents data of a person.</param>
        /// <returns>Returns the id of created record.</returns>
        public int CreateRecord(PersonalData personalData)
        {
            this.validator.Validate(personalData);

            var record = new FileCabinetRecord(this.list.Count + 1, personalData);

            this.list.Add(record);
            this.AddToIndex(record);

            return record.Id;
        }

        /// <summary>
        /// Edit record with the given id.
        /// </summary>
        /// <param name="id">Id of record to edit.</param>
        /// <param name="newData">New personal data for record.</param>
        /// <exception cref="ArgumentException">No record matching given id.</exception>
        public void EditRecord(int id, PersonalData newData)
        {
            this.validator.Validate(newData);

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.RemoveFromIndex(record);

                    record.FirstName = newData.FirstName;
                    record.LastName = newData.LastName;
                    record.DateOfBirth = newData.DateOfBirth;
                    record.SchoolGrade = newData.SchoolGrade;
                    record.AverageMark = newData.AverageMark;
                    record.ClassLetter = newData.ClassLetter;

                    this.AddToIndex(record);

                    return;
                }
            }

            throw new ArgumentException($"No record with {id} id.", nameof(id));
        }

        /// <summary>
        /// Find record by value of given field.
        /// </summary>
        /// <param name="fieldName">Name of field to search.</param>
        /// <param name="value">Value of <paramref name="fieldName"/> field to search.</param>
        /// <returns>Array of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByField(string fieldName, string value)
        {
            if (this.index.TryGetValue((fieldName.ToUpperInvariant(), value.ToString()), out var res))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(res);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
        }

        /// <summary>
        /// Get all stored records.
        /// </summary>
        /// <returns>All stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get count of stored records.
        /// </summary>
        /// <returns>Count of stored records.</returns>
        public (int have, int deleted) GetStat()
        {
            return (this.list.Count, 0);
        }

        /// <summary>
        /// Restore records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot contatining records to restore.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            int imported = 0;

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

                if (!this.list.Any(rec => rec.Id == record.Id))
                {
                    this.list.Add(record);
                    this.AddToIndex(record);
                    imported++;
                }
            }

            Console.WriteLine($"{imported} record were imported.");
        }

        /// <inheritdoc/>
        public bool Remove(int recordId)
        {
            var record = this.list.Where(r => r.Id == recordId).First();

            if (record is null)
            {
                return false;
            }

            this.RemoveFromIndex(record);
            this.list.Remove(record);

            return true;
        }

        /// <inheritdoc/>
        public int Purge() => 0;

        private void RemoveFromIndex(FileCabinetRecord record)
        {
            foreach (var (key, records) in this.index)
            {
                if (records.Contains(record))
                {
                    records.Remove(record);
                    if (records.Count == 0)
                    {
                        this.index.Remove(key);
                    }
                }
            }
        }

        private void AddToIndex(FileCabinetRecord record)
        {
            var properties = record.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string fieldName;
            object value;

            foreach (var property in properties)
            {
                fieldName = property.Name;
                if (fieldName.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                value = property.GetValue(record) !;

                var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                culture.DateTimeFormat.ShortDatePattern = FileCabinetRecord.OutputDateTimeFormat;
                culture.DateTimeFormat.LongTimePattern = string.Empty;
                culture.NumberFormat.NumberDecimalSeparator = ".";

                var key = (fieldName.ToUpperInvariant(), Convert.ToString(value, culture) !.Trim());

                if (this.index.ContainsKey(key))
                {
                    this.index[key].Add(record);
                }
                else
                {
                    this.index.Add(key, new List<FileCabinetRecord> { record });
                }
            }
        }
    }
}
