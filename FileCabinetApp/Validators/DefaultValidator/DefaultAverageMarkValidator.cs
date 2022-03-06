using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check average mark for valid value.
    /// </summary>
    public class DefaultAverageMarkValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum valid average mark value.
        /// </summary>
        private decimal averageMarkMinValue;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        private decimal averageMarkMaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAverageMarkValidator"/> class.
        /// </summary>
        /// <param name="averageMarkMinValue">Minimal valid value.</param>
        /// <param name="averageMarkMaxValue">Maximum valid value.</param>
        public DefaultAverageMarkValidator(decimal averageMarkMinValue, decimal averageMarkMaxValue)
        {
            this.averageMarkMinValue = averageMarkMinValue;
            this.averageMarkMaxValue = averageMarkMaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAverageMarkValidator"/> class.
        /// </summary>
        public DefaultAverageMarkValidator()
        {
            this.averageMarkMinValue = DefaultValidatorRules.AverageMarkMinValue;
            this.averageMarkMaxValue = DefaultValidatorRules.AverageMarkMaxValue;
        }

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="personalData">Containg average mark to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.AverageMark"/> is less than <see cref="averageMarkMinValue"/> or more than <see cref="averageMarkMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.AverageMark < this.averageMarkMinValue ||
                personalData.AverageMark > this.averageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.AverageMark)} must be between {this.averageMarkMinValue} and {this.averageMarkMaxValue}.");
            }
        }
    }
}
