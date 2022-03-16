using System.Collections.ObjectModel;
using System.Diagnostics;
using FileCabinetApp.Iterators;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetApp.Meters
{
    /// <summary>
    /// Measures how many ticks it took to method complete.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;
        private Stopwatch sw = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Service to work with.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            var st = new Stopwatch();
            this.service = service;
        }

        /// <inheritdoc/>
        public int CreateRecord(PersonalData personalData)
        {
            this.StartMeasurement();
            var res = this.service.CreateRecord(personalData);
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, PersonalData newData)
        {
            this.StartMeasurement();
            this.service.EditRecord(id, newData);
            this.StopMeasurement();
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByField(string fieldName, string value)
        {
            this.StartMeasurement();
            var res = this.service.FindByField(fieldName, value);
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.StartMeasurement();
            var res = this.service.GetRecords();
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public (int have, int deleted) GetStat()
        {
            this.StartMeasurement();
            var res = this.service.GetStat();
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.StartMeasurement();
            var res = this.service.MakeSnapshot();
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public int Purge()
        {
            this.StartMeasurement();
            var res = this.service.Purge();
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public bool Remove(int recordId)
        {
            this.StartMeasurement();
            var res = this.service.Remove(recordId);
            this.StopMeasurement();
            return res;
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.StartMeasurement();
            this.service.Restore(snapshot);
            this.StopMeasurement();
        }

        private void StartMeasurement()
        {
            this.sw.Reset();
            this.sw.Start();
        }

        private void StopMeasurement()
        {
            this.sw.Stop();
            var ticks = this.sw.ElapsedTicks;
            Console.WriteLine(
                "Method execution duration is {0} ticks.",
                ticks);
        }
    }
}
