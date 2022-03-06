using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check first name for valid value.
    /// </summary>
    public class DefaultFirstNameValidator : IRecordValidator
    {
        /// <summary>
        /// Minimal valid first name length.
        /// </summary>
        protected const int FirstNameMinLen = 2;

        /// <summary>
        /// Maximun valid last name length.
        /// </summary>
        protected const int FirstNameMaxLen = 60;

        /// <summary>
        /// Check first name for valid value.
        /// </summary>
        /// <param name="personalData">Containg first name to check.</param>
        /// <exception cref="ArgumentNullException">If <see cref="PersonalData.FirstName"/> is null.</exception>
        /// <exception cref="ArgumentException">If <see cref="PersonalData.FirstName"/> has invalid length or contains invalid symbols.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.FirstName is null)
            {
                throw new ArgumentNullException(nameof(personalData), $"{nameof(personalData.FirstName)} is null");
            }

            if (personalData.FirstName.Length < FirstNameMinLen ||
                personalData.FirstName.Length > FirstNameMaxLen ||
                !personalData.FirstName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(personalData.FirstName)} must contatin " +
                    $"from {FirstNameMinLen} " +
                    $"to {FirstNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }
    }
}
