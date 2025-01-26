using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace CLDV6212_POE_P2_ST10023767
{
    public class Queue
    {        
        /// <summary>
        /// Instance of the validation class 
        /// </summary>
        private Validation validate = new Validation();

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default constructor of the Queue class
        /// </summary>
        public Queue() { }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method will get the users data and store it in the correct format
        /// </summary>
        public void DeterminPath()
        {
            bool valid = false;

            //Notify the user that they can enter 'Exit' to quit 
            Console.WriteLine("Enter 'Exit' to quit...");
            do
            {
                //Ask the user for input 
                Console.Write("Please enter data for one of the following formats: \r\n" +
                    "(ID or Passport number:VaccinationCenter:VaccinationDate:VaccineSerialNumber) \r\n" +
                    "(VaccineBarcode:VaccinationDate:VaccinationCenter:ID or Passport number) \r\n");

                string userInput = Console.ReadLine();


                if (userInput.ToUpper() != "EXIT")
                {
                    var parts = userInput.Split(new[] { ':' }, StringSplitOptions.None);

                    if (parts.Length == 4)
                    {
                        if (parts[0].Length == 13 || parts[0].Length == 9)
                        {
                            //first format 
                            GetUserInputFirstFormat(parts);
                        }
                        else
                        {
                            if (parts[0].Length == 12)
                            {
                                //second format 
                                GetUserInputSecondFormat(parts);
                            }
                            else
                            {
                                Console.WriteLine("\r\nInvalid input, make sure that you enter your (ID or " +
                                    "PassportNumber/VaccineBarcode) correctley \r\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\r\nInvalid input format.");
                        valid = false;
                    }
                }
                else
                {
                    valid = true;
                }

            } while (!valid);

            Console.ReadLine();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method will get the data out of the first format
        /// </summary>
        /// <param name="parts"></param>
        public void GetUserInputFirstFormat(string[] parts)
        {
            int format = 1;
            var idOrPassportNumber = parts[0];
            var vaccinationCenter = parts[1];
            var vaccinationDate = parts[2];
            var vaccineSerialNumber = parts[3];
            string vaccineBarcode = null;

            if (validate.Validate_String(vaccinationCenter).valid && validate.Validate_Date(vaccinationDate).valid &&
                validate.ValidateInput(idOrPassportNumber).valid && validate.Validate_Serial_Number(vaccineSerialNumber).valid)
            {
                DateTime vaccinationDateConverted = Convert.ToDateTime(vaccinationDate);

                // Save to Azure Queue
                string messageContent = SendToAzureQueue(format, idOrPassportNumber, vaccinationCenter,
                    vaccinationDateConverted, vaccineSerialNumber, vaccineBarcode);

                //output
                Console.WriteLine($"\r\nInfo Added for {idOrPassportNumber}:");
                Console.WriteLine($"Vaccination Center: {vaccinationCenter}");
                Console.WriteLine($"Vaccination Date: {vaccinationDateConverted.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Vaccine Serial Number: {vaccineSerialNumber}");
            }
            else
            {
                string message = string.Empty;
                if (!validate.ValidateInput(idOrPassportNumber).valid)
                {
                    message += "\r\n" + validate.ValidateInput(idOrPassportNumber).report;
                }

                if (!validate.Validate_String(vaccinationCenter).valid)
                {
                    message += "\r\n" + validate.Validate_String(vaccinationCenter).report;
                }

                if (!validate.Validate_Date(vaccinationDate).valid)
                {
                    message += "\r\n" + validate.Validate_Date(vaccinationDate).report;
                }

                if (!validate.Validate_Serial_Number(vaccineSerialNumber).valid)
                {
                    message += "\r\n" + validate.Validate_Serial_Number(vaccineSerialNumber).report;
                }
                message += "\r\nPress enter to continue.";
                Console.WriteLine("\r\nData Input Error:" + message);
            }

            Console.ReadLine();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method will get the data out of the second fromat 
        /// </summary>
        /// <param name="parts"></param>
        public void GetUserInputSecondFormat(string[] parts)
        {
            int format = 2;
            var vaccineBarcode = parts[0];
            var vaccinationDate = parts[1];
            var vaccinationCenter = parts[2];
            var idOrPassportNumber = parts[3];
            string vaccineSerialNumber = null;


            if (validate.Validate_String(vaccinationCenter).valid && validate.Validate_Date(vaccinationDate).valid &&
                validate.ValidateInput(idOrPassportNumber).valid && validate.Validate_Barcode(vaccineBarcode).valid)
            {
                DateTime vaccinationDateConverted = Convert.ToDateTime(vaccinationDate);

                // Save to Azure Queue
                string messageContent = SendToAzureQueue(format, idOrPassportNumber, vaccinationCenter, 
                    vaccinationDateConverted, vaccineSerialNumber, vaccineBarcode);

                //output
                Console.WriteLine($"\r\nInfo Added for {idOrPassportNumber}:");
                Console.WriteLine($"Vaccination Center: {vaccinationCenter}");
                Console.WriteLine($"Vaccination Date: {vaccinationDateConverted.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Vaccine Barcode: {vaccineBarcode}");
            }
            else
            {
                string message = string.Empty;
                if (!validate.ValidateInput(idOrPassportNumber).valid)
                {
                    message += "\r\n" + validate.ValidateInput(idOrPassportNumber).report;
                }

                if (!validate.Validate_String(vaccinationCenter).valid)
                {
                    message += "\r\n" + validate.Validate_String(vaccinationCenter).report;
                }

                if (!validate.Validate_Date(vaccinationDate).valid)
                {
                    message += "\r\n" + validate.Validate_Date(vaccinationDate).report;
                }

                if (!validate.Validate_Barcode(vaccineBarcode).valid)
                {
                    message += "\r\n" + validate.Validate_Barcode(vaccineBarcode).report;
                }

                message += "\r\nPress enter to continue.";
                Console.WriteLine("\r\nData Input Error:" + message);
            }

            Console.ReadLine();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method will save data to the azure queue, using the correct format
        /// </summary>
        /// <param name="format"></param>
        /// <param name="idOrPassportNumber"></param>
        /// <param name="vaccinationCenter"></param>
        /// <param name="vaccinationDate"></param>
        /// <param name="vaccineSerialNumber"></param>
        /// <param name="vaccineBarcode"></param>
        /// <returns></returns>
        public string SendToAzureQueue(int format, string idOrPassportNumber, string vaccinationCenter,
            DateTime vaccinationDate, string vaccineSerialNumber, string vaccineBarcode)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=10023767;AccountKey=5UO99q6PUE" +
                "pymw27yowSjsGCSmjR411ep3tANjFqr5uzK0O4enudWKgLo4UvGgA7MUySGW7LQfhp+AStnhE/MA==;EndpointSuffix=co" +
                "re.windows.net";

            string queueName = "vaccine-queue";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            string messageContent = string.Empty;

            if (format == 1)
            {
                messageContent = $"{format.ToString()}:{idOrPassportNumber}:{vaccinationCenter}:" +
                    $"{vaccinationDate.ToString("yyyy-MM-dd")}:{vaccineSerialNumber}";
            }
            else if (format == 2)
            {
                messageContent = $"{format.ToString()}:{vaccineBarcode}:{vaccinationDate.ToString("yyyy-MM-dd")}:" +
                    $"{vaccinationCenter}:{idOrPassportNumber}";
            }

            CloudQueueMessage message = new CloudQueueMessage(messageContent);

            queue.AddMessage(message);  

            return messageContent;

        }
        //・♫-------------------------------------------------------------------------------------------------♫・//
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
