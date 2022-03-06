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
        protected const short SchoolGradeMinValue = 0;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        protected const short SchoolGradeMaxValue = 4;

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="personalData">Containg school grade to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="PersonalData.SchoolGrade"/> is less than <see cref="SchoolGradeMinValue"/> or more than <see cref="SchoolGradeMaxValue"/>.</exception>
        public void Validate(PersonalData personalData)
        {
            if (personalData.SchoolGrade < SchoolGradeMinValue ||
                personalData.SchoolGrade > SchoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(personalData),
                    $"{nameof(PersonalData.SchoolGrade)} must be between {SchoolGradeMinValue} and {SchoolGradeMaxValue}.");
            }
        }
    }
}
