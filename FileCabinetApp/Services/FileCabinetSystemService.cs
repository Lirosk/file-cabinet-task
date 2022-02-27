using System.Collections.ObjectModel;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Stores records with personal information; manages the creation, editing, finding the records.
    /// </summary>
    internal class FileCabinetSystemService : IFileCabinetService

    {
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Stream to save records.</param>
        public FileCabinetSystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Create record from given parameters.
        /// </summary>
        /// <param name="personalData">Represents data of a person.</param>
        /// <returns>Returns the id of created record.</returns>
        public int CreateRecord(PersonalData personalData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit record with the given id.
        /// </summary>
        /// <param name="id">Id of record to edit.</param>
        /// <param name="newData">New personal data for record.</param>
        /// <exception cref="ArgumentException">No record matching given id.</exception>
        public void EditRecord(int id, PersonalData newData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find record by value of given field.
        /// </summary>
        /// <param name="fieldName">Name of field to search.</param>
        /// <param name="value">Value of <paramref name="fieldName"/> field to search.</param>
        /// <returns>Array of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByField(string fieldName, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all stored records.
        /// </summary>
        /// <returns>All stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get count of stored records.
        /// </summary>
        /// <returns>Count of stored records.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes snapshot of present records.
        /// </summary>
        /// <returns>Snapshot of present records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
