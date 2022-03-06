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
        private int nameMinLen;

        /// <summary>
        /// Gets or sets maximum length for name.
        /// </summary>
        /// <value>Maximum length for name.</value>
        private int nameMaxLen;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLastNameValidator"/> class.
        /// </summary>
        /// <param name="nameMinLen">Minimal valid name length.</param>
        /// <param name="nameMaxLen">Maximum valid name length.</param>
        public CustomLastNameValidator(int nameMinLen, int nameMaxLen)
        {
            this.nameMinLen = nameMinLen;
            this.nameMaxLen = nameMaxLen;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLastNameValidator"/> class.
        /// </summary>
        public CustomLastNameValidator()
        {
            this.nameMinLen = CustomValidatorRules.NameMinLen;
            this.nameMaxLen = CustomValidatorRules.NameMaxLen;
        }

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="personalData">Containg last name to check.</param>
        /// <exception cref="ArgumentNullException">If <see cref="PersonalData.LastName"/> is null.</exception>
        /// <exception cref="ArgumentException">If <see cref="PersonalData.LastName"/> has invalid length or contains invalid symbols.</exception>
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
