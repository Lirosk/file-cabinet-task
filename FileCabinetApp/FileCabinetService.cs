using System.Globalization;
using System.Reflection;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.DateOfBirth = dateOfBirth;
                    record.SchoolGrade = schoolGrade;
                    record.AverageMark = averageMark;
                    record.ClassLetter = classLetter;
                    return;
                }
            }

            throw new ArgumentException($"No record with {id} id.", nameof(id));
        }

        public FileCabinetRecord[] FindByField(string fieldName, object value)
        {
            var field = typeof(FileCabinetRecord).GetProperty(fieldName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance) !;
            _ = field ?? throw new ArgumentException($"No field named {fieldName}");

            value = Convert.ChangeType(value, field.PropertyType, CultureInfo.InvariantCulture);

            return this.list.Where(record => field!.GetValue(record) !.Equals(value)).ToArray();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}