using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class for default validation personal data.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validated personal data.
        /// </summary>
        /// <param name="personalData">Personal data to validate.</param>
        public void Validate(PersonalData personalData)
        {
            new DefaultFirstNameValidator().Validate(personalData);
            new DefaultLastNameValidator().Validate(personalData);
            new DefaultDateOfBirthValidator().Validate(personalData);
            new DefaultSchoolGradeValidator().Validate(personalData);
            new DefaultAverageMarkValidator().Validate(personalData);
            new DefaultClassLetterValidator().Validate(personalData);
        }

        /// <summary>
        /// Check first name for valid value.
        /// </summary>
        /// <param name="personalData">Containg first name to check.</param>
        public void ValidateFirstName(PersonalData personalData)
            => new DefaultFirstNameValidator().Validate(personalData);

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="personalData">Containg last name to check.</param>
        public void ValidateLastName(PersonalData personalData)
            => new DefaultLastNameValidator().Validate(personalData);

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="personalData">Containg date of birth to check.</param>
        public void ValidateDateOfBirth(PersonalData personalData)
            => new DefaultDateOfBirthValidator().Validate(personalData);

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="personalData">Containg school grade to check.</param>
        public void ValidateSchoolGrade(PersonalData personalData)
            => new DefaultSchoolGradeValidator().Validate(personalData);

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="personalData">Containg average mark to check.</param>
        public void ValidateAverageMark(PersonalData personalData)
            => new DefaultAverageMarkValidator().Validate(personalData);

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="personalData">Containg class letter to check.</param>
        public void ValidateClassLetter(PersonalData personalData)
            => new DefaultClassLetterValidator().Validate(personalData);
    }
}
