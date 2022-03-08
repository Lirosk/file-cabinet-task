using FileCabinetApp.Validators;

namespace FileCabinetApp.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ValidatorBuilder"/>.
    /// </summary>
    public static class ValidatorBuilderExtensions
    {
        /// <summary>
        /// Creates validator with <see cref="DefaultValidatorRules"/> rules.
        /// </summary>
        /// <param name="builder">Builder to set validators.</param>
        /// <returns>returns validator with <see cref="DefaultValidatorRules"/> rules.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(DefaultValidatorRules.FirstNameMinLen, DefaultValidatorRules.FirstNameMaxLen)
                .ValidateLastName(DefaultValidatorRules.LastNameMinLen, DefaultValidatorRules.LastNameMaxLen)
                .ValidateDateOfBirth(DefaultValidatorRules.DateOfBirthMinValue, DefaultValidatorRules.DateOfBirthMaxValue)
                .ValidateSchoolGrade(DefaultValidatorRules.SchoolGradeMinValue, DefaultValidatorRules.SchoolGradeMaxValue)
                .ValidateAverageMark(DefaultValidatorRules.AverageMarkMinValue, DefaultValidatorRules.AverageMarkMaxValue)
                .ValidateClassLetter(DefaultValidatorRules.ClassLetterMinValue, DefaultValidatorRules.ClassLetterMaxValue)
                .Create();
        }

        /// <summary>
        /// Creates validator with <see cref="CustomValidatorRules"/> rules.
        /// </summary>
        /// <param name="builder">Builder to set validators.</param>
        /// <returns>returns validator with <see cref="CustomValidatorRules"/> rules.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(CustomValidatorRules.NameMinLen, CustomValidatorRules.NameMaxLen)
                .ValidateLastName(CustomValidatorRules.NameMinLen, CustomValidatorRules.NameMaxLen)
                .ValidateDateOfBirth(CustomValidatorRules.DateOfBirthMinValue, CustomValidatorRules.DateOfBirthMaxValue)
                .ValidateSchoolGrade(CustomValidatorRules.SchoolGradeMinValue, CustomValidatorRules.SchoolGradeMaxValue)
                .ValidateAverageMark(CustomValidatorRules.AverageMarkMinValue, CustomValidatorRules.AverageMarkMaxValue)
                .ValidateClassLetter(CustomValidatorRules.ClassLetterMinValue, CustomValidatorRules.ClassLetterMaxValue)
                .Create();
        }
    }
}
