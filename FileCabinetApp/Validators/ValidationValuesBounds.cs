namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Bounds for values to validate.
    /// </summary>
    public class ValidationValuesBounds
    {
        /// <summary>
        /// Gets or sets valid first name minimum and maximum lengths.
        /// </summary>
        /// <value></value>
        public Bounds<int> FirstName { get; set; }

        /// <summary>
        /// Gets or sets valid last name minimum and maximum lengths.
        /// </summary>
        /// <value>Valid last name minimum and maximum lengths.</value>
        public Bounds<int> LastName { get; set; }

        /// <summary>
        /// Gets or sets valid date of birth minimum and maximum values.
        /// </summary>
        /// <value>Valid date of birth minimum and maximum values.</value>
        public Bounds<DateTime> DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets valid school grade minimum and maximum values.
        /// </summary>
        /// <value>Valid school grade minimum and maximum values.</value>
        public Bounds<short> SchoolGrade { get; set; }

        /// <summary>
        /// Gets or sets valid average mark minimum and maximum values.
        /// </summary>
        /// <value>Valid average mark minimum and maximum values.</value>
        public Bounds<decimal> AverageMark { get; set; }

        /// <summary>
        /// Gets or sets valid class letter minimum and maximum values.
        /// </summary>
        /// <value>Valid class letter minimum and maximum values.</value>
        public Bounds<char> ClassLetter { get; set; }

        /// <summary>
        /// Represents bound for value to validate.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        public struct Bounds<T>
            where T : struct
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Bounds{T}"/> struct.
            /// </summary>
            /// <param name="min">Minimal valid value.</param>
            /// <param name="max">Maximum valid value.</param>
            public Bounds(T min, T max)
            {
                this.Min = min;
                this.Max = max;
            }

            /// <summary>
            /// Gets or sets minimal valid value.
            /// </summary>
            /// <value>Minimal valid value.</value>
            public T Min { get; set; }

            /// <summary>
            /// Gets or sets maximum valid value.
            /// </summary>
            /// <value>Maximum valid value.</value>
            public T Max { get; set; }
        }
    }
}
