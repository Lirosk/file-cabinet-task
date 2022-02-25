using System.Reflection;

namespace FileCabinetApp.FileCabinetServices
{
    /// <summary>
    /// Stores records with personal information; manages the creation, editing, finding the records.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<(string, string), List<FileCabinetRecord>> index = new ();

        /// <summary>
        /// Create record from given parameters.
        /// </summary>
        /// <param name="personalData">Represents data of a person.</param>
        /// <returns>Returns the id of created record.</returns>
        public int CreateRecord(PersonalData personalData)
        {
            this.Validate(personalData);

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
            this.Validate(newData);

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
        public FileCabinetRecord[] FindByField(string fieldName, object value)
        {
            if (this.index.TryGetValue((fieldName.ToUpperInvariant(), value as string ?? value.ToString() !), out var res))
            {
                return res.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Get all stored records.
        /// </summary>
        /// <returns>All stored records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Get count of stored records.
        /// </summary>
        /// <returns>Count of stored records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Checks for valid values in object parameter..
        /// </summary>
        /// <param name="personalData">Object parameter contains values to check for valid.</param>
        protected abstract void Validate(PersonalData personalData);

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
            var type = record.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

                var key = (fieldName.ToUpperInvariant(), value as string ?? value.ToString() !);

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
