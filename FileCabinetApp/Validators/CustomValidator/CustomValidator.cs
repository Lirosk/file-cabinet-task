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
                new CustomFirstNameValidator(),
                new CustomLastNameValidator(),
                new CustomDateOfBirthValidator(),
                new CustomSchoolGradeValidator(),
                new CustomAverageMarkValidator(),
                new CustomClassLetterValidator(),
            })
        {
        }
    }
}
