namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Builder than creates validator.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new ();

        /// <summary>
        /// Adds first name validating.
        /// </summary>
        /// <param name="firstNameMinLen">Minimal valid length of first name.</param>
        /// <param name="firstNameMaxLen">Maximum valid length of first name.</param>
        /// <returns>Returns itself.</returns>
        public ValidatorBuilder ValidateFirstName(int firstNameMinLen, int firstNameMaxLen)
        {
            this.validators.Add(new FirstNameValidator(firstNameMinLen, firstNameMaxLen));
            return this;
        }

        /// <summary>
        /// Adds last name validating.
        /// </summary>
        /// <param name="lastNameMinLen">Minimal valid length of last name.</param>
        /// <param name="lastNameMaxLen">Maximum valid length of last name.</param>
        /// <returns>Returns itself.</returns>
        public ValidatorBuilder ValidateLastName(int lastNameMinLen, int lastNameMaxLen)
        {
            this.validators.Add(new LastNameValidator(lastNameMinLen, lastNameMaxLen));
            return this;
        }

        /// <summary>
        /// Adds date of birth validating.
        /// </summary>
        /// <param name="dateOfBirthMinValue">Minimal date of birth valid value.</param>
        /// <param name="dateOfBirthMaxValue">Maximum date of birth valid value.</param>
        /// <returns>Returns itself.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime dateOfBirthMinValue, DateTime dateOfBirthMaxValue)
        {
            this.validators.Add(new DateOfBirthValidator(dateOfBirthMinValue, dateOfBirthMaxValue));
            return this;
        }

        /// <summary>
        /// Adds date of birth validating.
        /// </summary>
        /// <param name="schoolGradeMinValue">Minimal date of birth valid value.</param>
        /// <param name="schoolGradeMaxValue">Maximum date of birth valid value.</param>
        /// <returns>Returns itself.</returns>
        public ValidatorBuilder ValidateSchoolGrade(short schoolGradeMinValue, short schoolGradeMaxValue)
        {
            this.validators.Add(new SchoolGradeValidator(schoolGradeMinValue, schoolGradeMaxValue));
            return this;
        }

        /// <summary>
        /// Adds average mark validating.
        /// </summary>
        /// <param name="averageMarkMinValue">Minimal average mark valid value.</param>
        /// <param name="averageMarkMaxValue">Maximum average mark valid value.</param>
        /// <returns>Returns itself.</returns>
        public ValidatorBuilder ValidateAverageMark(decimal averageMarkMinValue, decimal averageMarkMaxValue)
        {
            this.validators.Add(new AverageMarkValidator(averageMarkMinValue, averageMarkMaxValue));
            return this;
        }

        /// <summary>
        /// Adds class letter validating.
        /// </summary>
        /// <param name="classLetterMinValue">Minimal class letter valid value.</param>
        /// <param name="classLetterMaxValue">Maximum class letter valid value.</param>
        /// <returns>Returns itself.</returns>
        public ValidatorBuilder ValidateClassLetter(char classLetterMinValue, char classLetterMaxValue)
        {
            this.validators.Add(new ClassLetterValidator(classLetterMinValue, classLetterMaxValue));
            return this;
        }

        /// <summary>
        /// Creates validator with setted parameters validators.
        /// </summary>
        /// <returns>Returns validator with setted parameters validators.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
