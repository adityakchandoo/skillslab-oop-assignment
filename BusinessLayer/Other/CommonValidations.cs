using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLayer.Other
{
    internal class CommonValidations
    {
        public static bool IsEmailValid(string input)
        {
            string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(input, Pattern);
        }

        public static bool IsNICValid(string input)
        {
            string Pattern = @"^.{1}[0-9]{6}.{7}$";
            return Regex.IsMatch(input, Pattern);
        }

        public static bool IsPhoneNumberValid(string input)
        {
            string Pattern = @"^(5\d{7}|[^5]\d{6})$";
            return Regex.IsMatch(input, Pattern);
        }
        public static bool IsDOBValid(DateTime dateOfBirth)
        {
            int age = CalculateAge(dateOfBirth);
            return age >= 16;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            // Adjust for leap year
            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            return age;
        }

        public static bool AreAllFilesValid(string[] fileNames)
        {
            string[] allowedExtensions =
            { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx"};

            return fileNames.All(fileName =>
                allowedExtensions.Contains(System.IO.Path.GetExtension(fileName).ToLower()));
        }
    }
}
