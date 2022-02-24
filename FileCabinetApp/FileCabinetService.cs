using System.Reflection;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<(string, string), List<FileCabinetRecord>> index = new ();

        public int CreateRecord(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            short schoolGrade,
            decimal averageMark,
            char classLetter)
        {
            FileCabinetRecord.ValidateFirstName(firstName);
            FileCabinetRecord.ValidateLastName(lastName);
            FileCabinetRecord.ValidateDateOfBirth(dateOfBirth);
            FileCabinetRecord.ValidateSchoolGrade(schoolGrade);
            FileCabinetRecord.ValidateAverageMark(averageMark);
            FileCabinetRecord.ValidateClassLetter(classLetter);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                SchoolGrade = schoolGrade,
                AverageMark = averageMark,
                ClassLetter = classLetter,
            };

            this.list.Add(record);
            this.AddToIndex(record);

            return record.Id;
        }

        public void EditRecord(
            int id,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            short schoolGrade,
            decimal averageMark,
            char classLetter)
        {
            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.RemoveFromIndex(record);

                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.DateOfBirth = dateOfBirth;
                    record.SchoolGrade = schoolGrade;
                    record.AverageMark = averageMark;
                    record.ClassLetter = classLetter;

                    this.AddToIndex(record);

                    return;
                }
            }

            throw new ArgumentException($"No record with {id} id.", nameof(id));
        }

        public FileCabinetRecord[] FindByField(string fieldName, object value)
        {
            if (this.index.TryGetValue((fieldName.ToUpperInvariant(), value as string ?? value.ToString() !), out var res))
            {
                return res.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

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
