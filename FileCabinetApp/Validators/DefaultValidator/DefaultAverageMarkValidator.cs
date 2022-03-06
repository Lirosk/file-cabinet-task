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
        protected const decimal AverageMarkMinValue = 0m;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        protected const decimal AverageMarkMaxValue = 10m;

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="personalData">Containg average mark to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.AverageMark"/> is less than <see cref="AverageMarkMinValue"/> or more than <see cref="AverageMarkMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.AverageMark < AverageMarkMinValue ||
                personalData.AverageMark > AverageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.AverageMark)} must be between {AverageMarkMinValue} and {AverageMarkMaxValue}.");
            }
        }
    }
}
