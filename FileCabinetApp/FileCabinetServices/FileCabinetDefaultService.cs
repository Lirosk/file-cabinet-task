using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetServices
{
    /// <summary>
    /// Service with default data validation.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates validator for personal data.
        /// </summary>
        /// <returns>Validator for personal data.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
