namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class for Default validation personal data.
    /// </summary>
    public class DefaultValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(),
                new LastNameValidator(),
                new DateOfBirthValidator(),
                new SchoolGradeValidator(),
                new AverageMarkValidator(),
                new ClassLetterValidator(),
            })
        {
        }
    }
}