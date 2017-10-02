using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.Backend
{
    class CryptoEngine
    {
        /// <summary>
        /// The current hash key used to generate the license key
        /// </summary>
        public static string HashKey { get; set; }

        /// <summary>
        /// Hashes a string using SHA256 algorthim 
        /// </summary>
        /// <param name="data"> The input string to be hashed</param>
        /// <returns>Hashed string using SHA256</returns>
        public static string Sha256Hash(string data)
        {
            if (data == null)
            {
                return null;
            }
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            foreach (byte t in hashData)
            {
                returnValue.Append(t.ToString());
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Return true if the input hash matches the stored hash data
        /// </summary>
        public static bool ValidateSha256(string input, string storedHashData)
        {
            string getHashInputData = Sha256Hash(input);
            return (String.CompareOrdinal(getHashInputData, storedHashData) == 0);
        }
    }
}
