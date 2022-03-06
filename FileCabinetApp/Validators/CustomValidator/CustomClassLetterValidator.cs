using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check class letter for valid value.
    /// </summary>
    public class CustomClassLetterValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum valid class letter value.
        /// </summary>
        protected const char ClassLetterMinValue = 'A';

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        protected const char ClassLetterMaxValue = 'G';

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="personalData">Containg class letter to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.ClassLetter"/> is less than <see cref="ClassLetterMinValue"/> or more than <see cref="ClassLetterMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            personalData.ClassLetter = char.ToUpperInvariant(personalData.ClassLetter);
            if (personalData.ClassLetter < ClassLetterMinValue ||
                personalData.ClassLetter > ClassLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.ClassLetter)} must be between {ClassLetterMinValue} and {ClassLetterMaxValue}.");
            }
        }
    }
}
