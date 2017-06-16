using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.Backend
{
    class LicenseEngine
    {
        #region Properties

        /// <summary>
        /// The user name for the current license holder
        /// </summary>
        public static string UserLicenseName;

        /// <summary>
        /// The curent license key
        /// </summary>
        public static string UserLicenseKey;

        /// <summary>
        /// True if the current license key is valid
        /// </summary>
        public static bool IsLicenseValid = false;

        /// <summary>
        /// The current hash key used to generate the license key
        /// </summary>
        public static string HashKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Hashes a string using SHA256 algorthim 
        /// </summary>
        /// <param name="data"> The input string to be hashed</param>
        /// <returns>Hashed string using SHA256</returns>
        public static string SHA256Hash(string data)
        {
            if (data == null)
            {
                return null;
            }
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Return true if the input hash matches the stored hash data
        /// </summary>
        public static bool ValidateSHA256(string input, string storedHashData)
        {
            string getHashInputData = SHA256Hash(input);
            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
