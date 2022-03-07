namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new();

        public ValidatorBuilder ValidateFirstName(int FirstNameMinLen, int FirstNameMaxLen)
        {
            this.validators.Add(new FirstNameValidator(FirstNameMinLen, FirstNameMaxLen));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int lastNameMinLen, int lastNameMaxLen)
        {
            this.validators.Add(new LastNameValidator(lastNameMinLen, lastNameMaxLen));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime dateOfBirthMinValue, DateTime dateOfBirthMaxValue)
        {
            this.validators.Add(new DateOfBirthValidator(dateOfBirthMinValue, dateOfBirthMaxValue));
            return this;
        }

        public ValidatorBuilder ValidateSchoolGrade(short schoolGradeMinValue, short schoolGradeMaxValue)
        {
            this.validators.Add(new SchoolGradeValidator(schoolGradeMinValue, schoolGradeMaxValue));
            return this;
        }

        public ValidatorBuilder ValidateAverageMark(decimal averageMarkMinValue, decimal averageMarkMaxValue)
        {
            this.validators.Add(new AverageMarkValidator(averageMarkMinValue, averageMarkMaxValue));
            return this;
        }

        public ValidatorBuilder ValidateClassLetter(char classLetterMinValue, char classLetterMaxValue)
        {
            this.validators.Add(new ClassLetterValidator(classLetterMinValue, classLetterMaxValue));
            return this;
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
