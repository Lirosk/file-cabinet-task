using Models;

namespace FileCabinetApp.Validators
{
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Validator for validating data.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        /// <summary>
        /// Validated personal data.
        /// </summary>
        /// <param name="personalData">Personal data to validate.</param>
        public void Validate(PersonalData personalData)
        {
            foreach (var validator in this.validators)
            {
                validator.Validate(personalData);
            }
        }
    }
}
