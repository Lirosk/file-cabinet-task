using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetServices
{
    /// <summary>
    /// Service with default data validation.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
