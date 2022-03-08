using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check first name for valid value.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        /// <summary>
        /// Minimal valid first name length.
        /// </summary>
        private int firstNameMinLen;

        /// <summary>
        /// Maximun valid last name length.
        /// </summary>
        private int firstNameMaxLen;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="firstNameMinLen">Minimal valid name length.</param>
        /// <param name="firstNameMaxLen">Maximum valid name length.</param>
        public FirstNameValidator(int firstNameMinLen, int firstNameMaxLen)
        {
            this.firstNameMinLen = firstNameMinLen;
            this.firstNameMaxLen = firstNameMaxLen;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        public FirstNameValidator()
        {
            this.firstNameMinLen = DefaultValidatorRules.FirstNameMinLen;
            this.firstNameMaxLen = DefaultValidatorRules.FirstNameMaxLen;
        }

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

            if (personalData.FirstName.Length < this.firstNameMinLen ||
                personalData.FirstName.Length > this.firstNameMaxLen ||
                !personalData.FirstName.All(c => char.IsLetter(c)))
            {
                throw new ArgumentException(
                    $"{nameof(personalData.FirstName)} must contatin " +
                    $"from {this.firstNameMinLen} " +
                    $"to {this.firstNameMaxLen} characters, " +
                    "and must not contain spaces, digits, special symbols.");
            }
        }
    }
}
