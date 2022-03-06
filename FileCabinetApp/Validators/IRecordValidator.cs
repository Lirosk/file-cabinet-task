using Models;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for validation personal data.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validated personal data.
        /// </summary>
        /// <param name="personalData">Personal data to validate.</param>
        public void Validate(PersonalData personalData);
    }
}
