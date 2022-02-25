namespace FileCabinetApp
{
    /// <summary>
    /// Represents parameter object for <see cref="FileCabinetRecord"/>.
    /// </summary>
    public struct PersonalData
    {
        /// <summary>
        /// Gets or sets first name of a person in record.
        /// </summary>
        /// <value>First name of a person in record.</value>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets last name of a person in record.
        /// </summary>
        /// <value>Last name of a person in record.</value>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth of a person in record.
        /// </summary>
        /// <value>Date of birth of a person in record.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets school grade of a person in record.
        /// </summary>
        /// <value>School grade of a person in record.</value>
        public short SchoolGrade { get; set; }

        /// <summary>
        /// Gets or sets averange mark of a person in record.
        /// </summary>
        /// <value>Average mark of a person in record.</value>
        public decimal AverageMark { get; set; }

        /// <summary>
        /// Gets or sets class letter of a person in record.
        /// </summary>
        /// <value>Class letter of a person in record.</value>
        public char ClassLetter { get; set; }
    }
}
