namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class for custom validation personal data.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(CustomValidatorRules.NameMinLen, CustomValidatorRules.NameMaxLen),
                new LastNameValidator(CustomValidatorRules.NameMinLen, CustomValidatorRules.NameMaxLen),
                new DateOfBirthValidator(CustomValidatorRules.DateOfBirthMinValue, CustomValidatorRules.DateOfBirthMaxValue),
                new SchoolGradeValidator(CustomValidatorRules.SchoolGradeMinValue, CustomValidatorRules.SchoolGradeMaxValue),
                new AverageMarkValidator(CustomValidatorRules.AverageMarkMinValue, CustomValidatorRules.AverageMarkMaxValue),
                new ClassLetterValidator(CustomValidatorRules.ClassLetterMinValue, CustomValidatorRules.ClassLetterMaxValue),
            })
        {
        }
    }
}
