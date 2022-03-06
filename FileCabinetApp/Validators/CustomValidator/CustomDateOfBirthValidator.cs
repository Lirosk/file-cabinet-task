using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check date of birth for valid value.
    /// </summary>
    public class CustomDateOfBirthValidator : IRecordValidator
    {
        /// <summary>
        /// Gets or sets minimum valid date of birth value.
        /// </summary>
        /// <value>Minimum valid date of birth value.</value>
        private DateTime dateOfBirthMinValue;

        /// <summary>
        /// Gets or sets maximum valid date of birth value.
        /// </summary>
        /// <value>Maximum valid date of birth value.</value>
        private DateTime dateOfBirthMaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="dateOfBirthMinValue">Minimal valid value.</param>
        /// <param name="dateOfBirthMaxValue">Maximum valid value.</param>
        public CustomDateOfBirthValidator(DateTime dateOfBirthMinValue, DateTime dateOfBirthMaxValue)
        {
            this.dateOfBirthMinValue = dateOfBirthMinValue;
            this.dateOfBirthMaxValue = dateOfBirthMaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDateOfBirthValidator"/> class.
        /// </summary>
        public CustomDateOfBirthValidator()
        {
            this.dateOfBirthMinValue = CustomValidatorRules.DateOfBirthMinValue;
            this.dateOfBirthMaxValue = CustomValidatorRules.DateOfBirthMaxValue;
        }

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="personalData">Containg date of birth to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.DateOfBirth"/> is less than <see cref="dateOfBirthMinValue"/> or more than <see cref="dateOfBirthMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.DateOfBirth < this.dateOfBirthMinValue ||
                personalData.DateOfBirth > this.dateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                   nameof(personalData),
                   $"{nameof(PersonalData.DateOfBirth)} must be between {this.dateOfBirthMinValue} and {this.dateOfBirthMaxValue}.");
            }
        }
    }
}
