namespace FileCabinetApp.Validators
{
    public class ValidationRules
    {
        public struct Bounds<T>
            where T : struct
        {
            public T Min { get; set; }

            public T Max { get; set; }

            public Bounds(T min, T max)
            {
                this.Min = min;
                this.Max = max;
            }
        }

        public Bounds<int> FirstName { get; set; }

        public Bounds<int> LastName { get; set; }

        public Bounds<DateTime> DateOfBirth { get; set; }

        public Bounds<short> SchoolGrade { get; set; }

        public Bounds<decimal> AverageMark { get; set; }

        public Bounds<char> ClassLetter { get; set; }
    }
}
