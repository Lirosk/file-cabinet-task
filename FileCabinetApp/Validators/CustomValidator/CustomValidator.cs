using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class for custom validation personal data.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validated personal data.
        /// </summary>
        /// <param name="personalData">Personal data to validate.</param>
        public void Validate(PersonalData personalData)
        {
            new CustomFirstNameValidator().Validate(personalData);
            new CustomLastNameValidator().Validate(personalData);
            new CustomDateOfBirthValidator().Validate(personalData);
            new CustomSchoolGradeValidator().Validate(personalData);
            new CustomAverageMarkValidator().Validate(personalData);
            new CustomClassLetterValidator().Validate(personalData);
        }
    }
}
