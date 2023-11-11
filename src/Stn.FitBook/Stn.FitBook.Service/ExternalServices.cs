using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Service
{
    public static class ExternalServices
    {
        public static bool AddPaymentCard(string cardNo)
        {
            return true;
        }


        public static bool PaymentCharge(Package packageInfo,int userId)
        {
            return true;
        }

        public static bool SendVerifyEmail(string email)
        {
            return true;
        }

        public static bool SendEmail(string email,string msg)
        {
            return true;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GeneratePassword()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            string confirmationcode = BitConverter.ToInt32(buffer, 0).ToString();
            confirmationcode = confirmationcode.Replace("-", "");
            confirmationcode = confirmationcode.Substring(0, 6);
            //string confirmationcode = "111111";

            return confirmationcode;
        }

    }
}
