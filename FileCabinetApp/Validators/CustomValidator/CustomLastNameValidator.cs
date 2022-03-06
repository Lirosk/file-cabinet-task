using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check last name for valid value.
    /// </summary>
    public class CustomLastNameValidator : IRecordValidator
    {
        /// <summary>
        /// Gets or sets minimum length for name.
        /// </summary>
        /// <value>Minimum length for name.</value>
        protected static int NameMinLen { get; set; } = 2;

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

            if (personalData.LastName.Length < NameMinLen)
            {
                throw new ArgumentException($"Name minimal length is {NameMinLen} symbols.");
            }
        }
    }
}
