using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetServices
{
    /// <summary>
    /// Service with custom data validation.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Creates validator for personal data.
        /// </summary>
        /// <returns>Validator for personal data.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
