using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check last name for valid value.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum valid last name length.
        /// </summary>
        private int lastNameMinLen;

        /// <summary>
        /// Maximum valid last name length.
        /// </summary>
        private int lastNameMaxLen;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="lastNameMinLen">Minimal valid name length.</param>
        /// <param name="lastNameMaxLen">Maximum valid name length.</param>
        public LastNameValidator(int lastNameMinLen, int lastNameMaxLen)
        {
            this.lastNameMinLen = lastNameMinLen;
            this.lastNameMaxLen = lastNameMaxLen;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        public LastNameValidator()
        {
            this.lastNameMinLen = DefaultValidatorRules.LastNameMinLen;
            this.lastNameMaxLen = DefaultValidatorRules.LastNameMaxLen;
        }

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

            if (personalData.LastName.Length < this.lastNameMinLen ||
                personalData.LastName.Length > this.lastNameMaxLen ||
                !personalData.LastName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(personalData.LastName)} must contatin " +
                    $"from {this.lastNameMinLen} " +
                    $"to {this.lastNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }
    }
}
