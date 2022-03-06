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
        private int nameMinLen;

        /// <summary>
        /// Gets or sets maximum length for name.
        /// </summary>
        /// <value>Maximum length for name.</value>
        private int nameMaxLen;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFirstNameValidator"/> class.
        /// </summary>
        /// <param name="nameMinLen">Minimal valid name length.</param>
        /// <param name="nameMaxLen">Maximum valid name length.</param>
        public CustomFirstNameValidator(int nameMinLen, int nameMaxLen)
        {
            this.nameMinLen = nameMinLen;
            this.nameMaxLen = nameMaxLen;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFirstNameValidator"/> class.
        /// </summary>
        public CustomFirstNameValidator()
        {
            this.nameMinLen = CustomValidatorRules.NameMinLen;
            this.nameMaxLen = CustomValidatorRules.NameMaxLen;
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

            if (personalData.FirstName.Length < this.nameMinLen)
            {
                throw new ArgumentException($"Name minimal length is {this.nameMinLen} symbols, max is {this.nameMaxLen}.");
            }
        }
    }
}
