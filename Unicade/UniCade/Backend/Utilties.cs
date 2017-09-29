using System;
using System.Text.RegularExpressions;
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
                if (!Char.IsDigit(c))
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
            return (str.IndexOfAny(new[] { '|', '<', '>' }) != -1);
        }

        /// <summary>
        /// Remove and replace all invalid chars from the input string
        /// </summary>
        internal static string RemoveInvalidChars(string str)
        {
            str = Regex.Replace(str, @"\t|\n|\r", " ");
            return str.Replace("\"", "");
        }

        /// <summary>
        /// Attempt to parse an ESRB descriptor enum from a string
        /// Return Null enum is not found
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static Enums.EsrbDescriptors ParseEsrbDescriptor(string str)
        {
            //Trim leading and trailing spaces before comparing the string values
            str = str.Trim();
            foreach (Enums.EsrbDescriptors descriptor in Enum.GetValues(typeof(Enums.EsrbDescriptors)))
            {
                if (string.Equals(str, descriptor.GetStringValue(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return descriptor;
                }
            }
            return Enums.EsrbDescriptors.Null;
        }

        /// <summary>
        /// Attempt to parse an ESRB descriptor enum from a string
        /// Return Null enum is not found
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static Enums.EsrbRatings ParseEsrbRating(string str)
        {
            if (str != null)
            {
                if (str.IndexOf("Everyone 10", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Enums.EsrbRatings.Everyone10;
                }
                if (str.IndexOf("Everyone", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Enums.EsrbRatings.Everyone;
                }
                if (str.IndexOf("Teen", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Enums.EsrbRatings.Teen;
                }
                if (str.IndexOf("Mature", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Enums.EsrbRatings.Mature;
                }
                if (str.IndexOf("Adult", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Enums.EsrbRatings.Ao;
                }
            }
            return Enums.EsrbRatings.Null;
        }


    }
}
