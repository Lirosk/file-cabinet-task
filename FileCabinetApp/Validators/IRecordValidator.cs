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
        public void ValidateParameters(PersonalData personalData)
        {
            this.ValidateFirstName(personalData.FirstName);
            this.ValidateLastName(personalData.LastName);
            this.ValidateDateOfBirth(personalData.DateOfBirth);
            this.ValidateSchoolGrade(personalData.SchoolGrade);
            this.ValidateAverageMark(personalData.AverageMark);
            this.ValidateClassLetter(personalData.ClassLetter);
        }

        /// <summary>
        /// Check first name for valid value.
        /// </summary>
        /// <param name="firstName">Value to check.</param>
        public void ValidateFirstName(string firstName);

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="lastName">Value to check.</param>
        public void ValidateLastName(string lastName);

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="dateOfBirth">Value to check.</param>
        public void ValidateDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="schoolGrade">Value to check.</param>
        public void ValidateSchoolGrade(short schoolGrade);

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="averageMark">Value to check.</param>
        public void ValidateAverageMark(decimal averageMark);

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="classLetter">Value to check.</param>
        public void ValidateClassLetter(char classLetter);
    }
}
