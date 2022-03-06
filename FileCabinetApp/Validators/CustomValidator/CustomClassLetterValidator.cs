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
        private char classLetterMinValue;

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        private char classLetterMaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomClassLetterValidator"/> class.
        /// </summary>
        /// <param name="classLetterMinValue">Minimal valid value.</param>
        /// <param name="classLetterMaxValue">Maximum valid value.</param>
        public CustomClassLetterValidator(char classLetterMinValue, char classLetterMaxValue)
        {
            this.classLetterMinValue = classLetterMinValue;
            this.classLetterMaxValue = classLetterMaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomClassLetterValidator"/> class.
        /// </summary>
        public CustomClassLetterValidator()
        {
            this.classLetterMinValue = CustomValidatorRules.ClassLetterMinValue;
            this.classLetterMaxValue = CustomValidatorRules.ClassLetterMaxValue;
        }

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="personalData">Containg class letter to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.ClassLetter"/> is less than <see cref="classLetterMinValue"/> or more than <see cref="classLetterMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            personalData.ClassLetter = char.ToUpperInvariant(personalData.ClassLetter);
            if (personalData.ClassLetter < this.classLetterMinValue ||
                personalData.ClassLetter > this.classLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.ClassLetter)} must be between {this.classLetterMinValue} and {this.classLetterMaxValue}.");
            }
        }
    }
}
