using System.Collections.ObjectModel;

using Models;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Stores records with personal information; manages the creation, editing, finding the records.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Create record from given parameters.
        /// </summary>
        /// <param name="personalData">Represents data of a person.</param>
        /// <returns>Returns the id of created record.</returns>
        public int CreateRecord(PersonalData personalData);

        /// <summary>
        /// Edit record with the given id.
        /// </summary>
        /// <param name="id">Id of record to edit.</param>
        /// <param name="newData">New personal data for record.</param>
        /// <exception cref="ArgumentException">No record matching given id.</exception>
        public void EditRecord(int id, PersonalData newData);

        /// <summary>
        /// Find record by value of given field.
        /// </summary>
        /// <param name="fieldName">Name of field to search.</param>
        /// <param name="value">Value of <paramref name="fieldName"/> field to search.</param>
        /// <returns>Array of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByField(string fieldName, string value);

        /// <summary>
        /// Get all stored records.
        /// </summary>
        /// <returns>All stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Get count of stored records.
        /// </summary>
        /// <returns>Count of stored records.</returns>
        public (int have, int deleted) GetStat();

        /// <summary>
        /// Makes snapshot of present records.
        /// </summary>
        /// <returns>Snapshot of present records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restore records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot contatining records to restore.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="recordId">Id of record to remove.</param>
        /// <returns>True if record has been deleted.</returns>
        public bool Remove(int recordId);

        /// <summary>
        /// Remove records marked as deleted.
        /// </summary>
        /// <returns>Count of deleted records.</returns>
        public int Purge();
    }
}
