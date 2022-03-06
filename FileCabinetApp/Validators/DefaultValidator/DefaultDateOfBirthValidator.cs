using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check date of birth for valid value.
    /// </summary>
    public class DefaultDateOfBirthValidator : IRecordValidator
    {
        /// <summary>
        /// Gets or sets minimum valid date of birth value.
        /// </summary>
        /// <value>Minimum valid date of birth value.</value>
        protected static DateTime DateOfBirthMinValue { get; set; } = new (1950, 1, 1);

        /// <summary>
        /// Gets maximum valid date of birth value.
        /// </summary>
        /// <value>Maximum valid date of birth value.</value>
        protected static DateTime DateOfBirthMaxValue { get => DateTime.Now; }

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="personalData">Containg date of birth to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.DateOfBirth"/> is less than <see cref="DateOfBirthMinValue"/> or more than <see cref="DateOfBirthMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.DateOfBirth < DateOfBirthMinValue ||
                personalData.DateOfBirth > DateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.DateOfBirth)} must be between {DateOfBirthMinValue} and {DateOfBirthMaxValue}.");
            }
        }
    }
}
