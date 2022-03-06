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
    }
}
