using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCade.Constants;

namespace UniCade.Backend
{
    internal class Utilties
    {
        /// <summary>
        /// Verify that a string contains only numeric chars
        /// </summary>
        public static bool IsAllDigits(string str)
        {
            foreach (var c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if a string contains any invalid chars
        /// </summary>
        /// <param name="str">The string to validate</param>
        /// <returns>false if the string contains any invalid characters</returns>
        public static bool CheckForInvalidChars(string str)
        {
            return (str.IndexOfAny(ConstValues.InvalidChars) != -1);
        }


        /// <summary>
        /// Check if a string contains any invalid chars
        /// </summary>
        /// <param name="str">The string to validate</param>
        /// <returns>false if the string contains any invalid characters</returns>
        public static bool CheckForInvalidSplitChars(string str)
        {
            return (str.IndexOfAny(new char[] {'|'}) != -1);
        }
    }
}
