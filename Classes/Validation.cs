using System;
using System.Linq;

namespace CLDV6212_POE_P2_ST10023767
{
    public class Validation
    {
        /// <summary>
        /// This method checks if the input is a valid string
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public (bool valid, string report) Validate_String(string userInput)
        {
            try
            {
                if ((!userInput.Equals(string.Empty)) && (!userInput.Equals(null)))
                {
                    return (true,"Valid input");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Invalid vaccination center, can't be empty or null");
            }
            return (false, "Invalid vaccination center, can't be empty or null");
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method checks if the input is a valid integer 
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public bool Validate_Integer(string userInput)
        {
            bool valid = false;

            try
            {
                if (Int32.TryParse(userInput, out int number) == true)
                {
                    if (number >= 0)
                    {
                        valid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                valid = false;
            }

            return valid;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method checks if the date is valid
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public (bool valid, string report) Validate_Date(string userInput)
        {
            try
            {
                if (DateTime.TryParse(userInput, out DateTime date) == true)
                {
                    return (true, "Valid Date");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Invalid Date, Date should be in one of the following formats: (MM/DD/YYYY)," +
                    "(MM-DD-YYY) or (DD Month YYYY)");
            }

            return (false, "Invalid Date, Date should be in one of the following formats: (MM/DD/YYYY)," +
                "(MM-DD-YYY) or (DD Month YYYY)");
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method checks if the user input is a valid id or passport number
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public (bool valid, string report) ValidateInput(string userInput)
        {
            if (IsValidId(userInput))
            {
                return (true, "ID");
            }
            else
            {
                if (IsValidPassport(userInput))
                {
                    return (true, "Passport");
                }
                else
                {
                    return (false, "Invalid ID or Passport Number.");
                }
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method checks if the ID is valid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsValidId(string id)
        {
            // Check if the ID has the correct length
            if (id.Length != 13)
            {
                return false;
            }

            // Ensure that all characters in the ID are digits
            foreach (char c in id)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            // Extract the birthdate portion from the ID (ddMMyyyy)
            string birthdateString = id.Substring(0, 6);

            // Convert the birthdate portion to a valid date format
            if (!DateTime.TryParseExact(birthdateString, "yyMMdd", null, System.Globalization.DateTimeStyles.None,
                out DateTime birthdate))
            {
                return false;
            }

            // Check if the resulting date is not greater than the current year
            int currentYear = DateTime.Now.Year;
            if (birthdate.Year > currentYear)
            {
                return false;
            }

            return true;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This will checks if the passport is valid 
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        private bool IsValidPassport(string passport)
        {
            return passport.Length == 9 &&
                   char.IsLetter(passport[0]) &&
                   passport.Substring(1).All(char.IsDigit);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method checks if the serial number is valid
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public (bool valid, string report) Validate_Serial_Number(string userInput)
        {
            try
            {
                if (userInput.Length == 10 && userInput.All(char.IsDigit))
                {
                    return (true,"Valid Serial Number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Invalid Serial Number, Serial Number must be 10 digits and only digits.");
            }

            return (false, "Invalid Serial Number, Serial Number must be 10 digits and only digits.");
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method checks if the barcode is valid
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public (bool valid, string report) Validate_Barcode(string userInput)
        {
            try
            {
                if (userInput.Length == 12 && userInput.All(char.IsDigit))
                {
                    return (true, "Valid Barcode");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Invalid Barcode, Barcode must be 12 digits and only digits.");
            }
            return (false, "Invalid Barcode, Barcode must be 12 digits and only digits.") ;
        }
        //・♫-------------------------------------------------------------------------------------------------♫・//
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
