using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetServices
{
    /// <summary>
    /// Service with custom data validation.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }
    }
}
