using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check last name for valid value.
    /// </summary>
    public class DefaultLastNameValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum valid last name length.
        /// </summary>
        protected const int LastNameMinLen = 2;

        /// <summary>
        /// Maximum valid last name length.
        /// </summary>
        protected const int LastNameMaxLen = 60;

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="personalData">Containg last name to check.</param>
        /// <exception cref="ArgumentNullException">If <see cref="PersonalData.LastName"/> is null.</exception>
        /// <exception cref="ArgumentException">If <see cref="PersonalData.LastName"/> has invalid length or contains invalid symbols.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.LastName is null)
            {
                throw new ArgumentNullException(nameof(personalData), $"{nameof(personalData.LastName)} is null");
            }

            if (personalData.LastName.Length < LastNameMinLen ||
                personalData.LastName.Length > LastNameMaxLen ||
                !personalData.LastName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(personalData.LastName)} must contatin " +
                    $"from {LastNameMinLen} " +
                    $"to {LastNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }
    }
}
