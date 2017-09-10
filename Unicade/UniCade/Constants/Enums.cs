using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.Constants
{
    public static class Enums
    {
        #region Enums

        /// <summary>
        /// Enum Values for ESRB content ratings
        /// </summary>
        public enum Esrb
        {
            [StringValue("")]
            Null,
            [StringValue("Everyone")]
            Everyone,
            [StringValue("Everyone 10+")]
            Everyone10,
            [StringValue("Teen")]
            Teen,
            [StringValue("Mature")]
            Mature,
            [StringValue("Adults Only (Ao)")]
            Ao
        }

        /// <summary>
        /// Enum Values for Unicade user types
        /// </summary>
        public enum UserType
        {
            LocalAccount,
            CloudAccount
        }

        #endregion

            #region Helper Methods

            /// <summary>
            /// Return the StringValue atribute from the enum
            /// </summary>
            /// <param name="enumValue">The current enum</param>
            /// <returns>the string value for the current enum</returns>
        public static string GetStringValue(this Enum enumValue)
        {
            //Fetch the type and field info
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());

            //Fetch the string value attributes 
            StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            //Return the StringValue attribute
            if (attributes.Length > 0)
            {
                return attributes[0].StringValue;
            }
                return fieldInfo.Name;
        }

        /// <summary>
        /// Define a StringValue attribute for the Enum class
        /// </summary>
        public class StringValueAttribute : Attribute
        {
            public string StringValue { get; private set; }

            public StringValueAttribute(string value)
            {
                StringValue = value;
            }
        }

        /// <summary>
        /// Convert a string value into an ESRB enum value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Esrb ConvertStringToEsrbEnum(string text)
        {
            if (text.Equals("") || text.Contains("Null") || text.Contains("null") || text.Contains("None") || text.Equals(" "))
            {
                return Esrb.Null;
            }
            else if (text.Equals("Everyone"))
            {
                return Esrb.Everyone;
            }
            else if (text.Equals("Everyone 10+"))
            {
                return Esrb.Everyone;
            }
            else if (text.Equals("Teen"))
            {
                return Esrb.Teen;
            }
            else if (text.Equals("Mature"))
            {
                return Esrb.Mature;
            }
            else if (text.Contains("Adults Only"))
            {
                return Esrb.Ao;
            }
            else
            {
                throw new ArgumentException("Invalid ESRB Rating");
            }
        }

        #endregion
    }
}
