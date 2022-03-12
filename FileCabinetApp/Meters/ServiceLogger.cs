using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.Meters
{
    /// <summary>
    /// Logs all method calls and their args.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private readonly string dateTimeFormat = FileCabinetRecord.InputDateTimeFormat + " hh:mm";

        private IFileCabinetService service;
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        /// <param name="writer">Writer to log all calls.</param>
        public ServiceLogger(IFileCabinetService service, TextWriter writer)
        {
            this.service = service;
            this.writer = writer;
        }

        /// <inheritdoc/>
        public int CreateRecord(PersonalData personalData)
        {
            this.writer.WriteLine(
                "{0}UTC - Calling CreateRecord() with personalData = {1}.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                personalData);

            var res = this.service.CreateRecord(personalData);

            this.writer.WriteLine(
                "{0}UTC - Create() returned {0}.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                res);

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, PersonalData newData)
        {
            this.writer.WriteLine(
                "{0}UTC - Calling EditRecord() with id = {1}, newData = {2}",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                id,
                newData);

            this.service.EditRecord(id, newData);

            this.writer.WriteLine(
                "{0}UTC - EditRecord() completed.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            this.writer.Flush();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByField(string fieldName, object value)
        {
            this.writer.WriteLine(
                "{0}UTC - Calling FindByField() with fieldName = {1}, value = {2}.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                fieldName,
                value);

            var res = this.service.FindByField(fieldName, value);

            this.writer.WriteLine(
                "{0}UTC - FindByField() returned collection with {0} elements.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                res.Count);

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.writer.WriteLine(
                "{0}UTC - Calling FindByField().",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            var res = this.service.GetRecords();

            this.writer.WriteLine(
                "{0}UTC - GetRecords() returned collection with {0} elements.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                res.Count);

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public (int have, int deleted) GetStat()
        {
            this.writer.WriteLine(
                "{0}UTC - Calling GetStat().",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            var res = this.service.GetStat();

            this.writer.WriteLine(
                "{0}UTC - GetStat() returned ({1}, {2}).",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                res.have,
                res.deleted);

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.writer.WriteLine(
                "{0}UTC - Calling MakeSnapshot().",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            var res = this.service.MakeSnapshot();

            this.writer.WriteLine(
                "{0}UTC - MakeSnapshot() completed.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public int Purge()
        {
            this.writer.WriteLine(
                "{0}UTC - Calling Purge().",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            var res = this.service.Purge();

            this.writer.WriteLine(
                "{0}UTC - Purge() completed.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                res);

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public bool Remove(int recordId)
        {
            this.writer.WriteLine(
                "{0}UTC - Calling Remove() with recordId = {1}.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                recordId);

            var res = this.service.Remove(recordId);

            this.writer.WriteLine(
                "{0}UTC - Remove() returned {1}.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture),
                res);

            this.writer.Flush();
            return res;
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.writer.WriteLine(
                "{0}UTC - Calling Restore().",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            this.service.Restore(snapshot);

            this.writer.WriteLine(
                "{0}UTC - Restore() completed.",
                DateTime.UtcNow.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture));

            this.writer.Flush();
        }
    }
}
