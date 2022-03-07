using FileCabinetApp.Validators;

namespace FileCabinetApp.Extensions
{
    public static class ValidatorBuilderExtensions
    {
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
