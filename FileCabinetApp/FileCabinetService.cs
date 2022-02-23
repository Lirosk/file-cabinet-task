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
            const int firstNameMinLen = 2;
            const int firstNameMaxLen = 60;
            const int lastNameMinLen = 2;
            const int lastNameMaxLen = 60;
            DateTime dateOfBirthMinValue = new DateTime(1950, 1, 1);
            DateTime dateOfBirthMaxValue = DateTime.Now;
            const short schoolGradeMinValue = 1;
            const short schoolGradeMaxValue = 11;
            const decimal averageMarkMinValue = 0m;
            const decimal averageMarkMaxValue = 10m;
            const char classLetterMinValue = 'A';
            const char classLetterMaxValue = 'E';

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (firstName.Length < firstNameMinLen ||
                firstName.Length > firstNameMaxLen ||
                firstName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"{nameof(firstName)} must contatin " +
                    $"from {firstNameMinLen} " +
                    $"to {firstNameMaxLen} characters, " +
                    "and must not contain spaces.");
            }

            if (lastName.Length < lastNameMinLen ||
                lastName.Length > lastNameMaxLen ||
                lastName.Contains(' ', StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"{nameof(lastName)} must contatin " +
                    $"from {lastNameMinLen} " +
                    $"to {lastNameMaxLen} characters, " +
                    "and must not contain spaces.");
            }

            if (dateOfBirth < dateOfBirthMinValue ||
                dateOfBirth > dateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
            }

            if (schoolGrade < schoolGradeMinValue ||
                schoolGrade > schoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(schoolGrade));
            }

            if (averageMark < averageMarkMinValue ||
                averageMark > averageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(averageMark));
            }

            classLetter = char.ToUpperInvariant(classLetter);
            if (classLetter < classLetterMinValue ||
                classLetter > classLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(classLetter));
            }

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