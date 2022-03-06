using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check first name for valid value.
    /// </summary>
    public class CustomFirstNameValidator : IRecordValidator
    {
        /// <summary>
        /// Gets or sets minimum length for name.
        /// </summary>
        /// <value>Minimum length for name.</value>
        protected static int NameMinLen { get; set; } = 2;

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

            if (personalData.FirstName.Length < NameMinLen)
            {
                throw new ArgumentException($"Name minimal length is {NameMinLen} symbols.");
            }
        }
    }
}
