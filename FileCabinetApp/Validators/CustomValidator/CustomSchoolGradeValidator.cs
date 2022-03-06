using Models;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Check school grade for valid value.
    /// </summary>
    public class CustomSchoolGradeValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum valid school grade value.
        /// </summary>
        private short schoolGradeMinValue;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        private short schoolGradeMaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSchoolGradeValidator"/> class.
        /// </summary>
        /// <param name="schoolGradeMinValue">Minimal valid value.</param>
        /// <param name="schoolGradeMaxValue">Maximum valid value.</param>
        public CustomSchoolGradeValidator(short schoolGradeMinValue, short schoolGradeMaxValue)
        {
            this.schoolGradeMinValue = schoolGradeMinValue;
            this.schoolGradeMaxValue = schoolGradeMaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSchoolGradeValidator"/> class.
        /// </summary>
        public CustomSchoolGradeValidator()
        {
            this.schoolGradeMinValue = CustomValidatorRules.SchoolGradeMinValue;
            this.schoolGradeMaxValue = CustomValidatorRules.SchoolGradeMaxValue;
        }

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="personalData">Containg school grade to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.SchoolGrade"/> is less than <see cref="schoolGradeMinValue"/> or more than <see cref="schoolGradeMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.SchoolGrade < this.schoolGradeMinValue ||
                personalData.SchoolGrade > this.schoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.SchoolGrade)} must be between {this.schoolGradeMinValue} and {this.schoolGradeMaxValue}.");
            }
        }
    }
}
