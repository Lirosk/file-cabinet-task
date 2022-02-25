﻿namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class for custom validation personal data.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum valid school grade value.
        /// </summary>
        protected const short SchoolGradeMinValue = 0;

        /// <summary>
        /// Maximum valid school grade value.
        /// </summary>
        protected const short SchoolGradeMaxValue = 4;

        /// <summary>
        /// Minimum valid average mark value.
        /// </summary>
        protected const decimal AverageMarkMinValue = 0m;

        /// <summary>
        /// Maximum valid average mark value.
        /// </summary>
        protected const decimal AverageMarkMaxValue = 5m;

        /// <summary>
        /// Minimum valid class letter value.
        /// </summary>
        protected const char ClassLetterMinValue = 'A';

        /// <summary>
        /// Maximum valid class letter value.
        /// </summary>
        protected const char ClassLetterMaxValue = 'G';

        /// <summary>
        /// Gets or sets minimum length for name.
        /// </summary>
        /// <value>Minimum length for name.</value>
        protected static int NameMinLen { get; set; } = 2;

        /// <summary>
        /// Gets or sets minimum valid date of birth value.
        /// </summary>
        /// <value>Minimum valid date of birth value.</value>
        protected static DateTime DateOfBirthMinValue { get; set; } = new (1950, 1, 1);

        /// <summary>
        /// Gets or sets maximum valid date of birth value.
        /// </summary>
        /// <value>Maximum valid date of birth value.</value>
        protected static DateTime DateOfBirthMaxValue { get; set; } = new (2016, 1, 1);

        /// <summary>
        /// Check first name for valid value.
        /// </summary>
        /// <param name="firstName">Value to check.</param>
        /// <exception cref="ArgumentNullException">If firstName param is null.</exception>
        /// <exception cref="ArgumentException">If firstName has invalid length or contains invalid symbols.</exception>
        public void ValidateFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (firstName.Length < NameMinLen)
            {
                throw new ArgumentException($"Each name minimal length is {NameMinLen} symbols.");
            }
        }

        /// <summary>
        /// Check last name for valid value.
        /// </summary>
        /// <param name="lastName">Value to check.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="lastName"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="lastName"/> has invalid length or contains invalid symbols.</exception>
        public void ValidateLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (lastName.Length < NameMinLen)
            {
                throw new ArgumentException($"Each name minimal length is {NameMinLen} symbols.");
            }
        }

        /// <summary>
        /// Check date of birth for valid value.
        /// </summary>
        /// <param name="dateOfBirth">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="dateOfBirth"/> is less than <see cref="DateOfBirthMinValue"/> or more than <see cref="DateOfBirthMaxValue"/>.</exception>
        public void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < DateOfBirthMinValue ||
                dateOfBirth > DateOfBirthMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
            }
        }

        /// <summary>
        /// Check school grade for valid value.
        /// </summary>
        /// <param name="schoolGrade">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="schoolGrade"/> is less than <see cref="SchoolGradeMinValue"/> or more than <see cref="SchoolGradeMaxValue"/>.</exception>
        public void ValidateSchoolGrade(short schoolGrade)
        {
            if (schoolGrade < SchoolGradeMinValue ||
                schoolGrade > SchoolGradeMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(schoolGrade));
            }
        }

        /// <summary>
        /// Check average mark for valid value.
        /// </summary>
        /// <param name="averageMark">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="averageMark"/> is less than <see cref="AverageMarkMinValue"/> or more than <see cref="AverageMarkMaxValue"/>.</exception>
        public void ValidateAverageMark(decimal averageMark)
        {
            if (averageMark < AverageMarkMinValue ||
                averageMark > AverageMarkMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(averageMark));
            }
        }

        /// <summary>
        /// Check class letter for valid value.
        /// </summary>
        /// <param name="classLetter">Value to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="classLetter"/> is not between <see cref="ClassLetterMinValue"/> and <see cref="ClassLetterMaxValue"/> values.</exception>
        public void ValidateClassLetter(char classLetter)
        {
            if (classLetter < ClassLetterMinValue ||
                classLetter > ClassLetterMaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(classLetter));
            }
        }
    }
}
