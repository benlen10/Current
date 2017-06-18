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
        public enum ESRB
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
            [StringValue("AO (Adults Only)")]
            AO
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
            else
            {
                return fieldInfo.Name;
            }
        }

        /// <summary>
        /// Define a StringValue attribute for the Enum class
        /// </summary>
        public class StringValueAttribute : Attribute
        {
            public string StringValue { get; private set; }

            public StringValueAttribute(string value)
            {
                this.StringValue = value;
            }
        }

        /// <summary>
        /// Convert a string value into an ESRB enum value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ESRB ConvertStringToEsrbEnum(string text)
        {
            if (text.Equals("Everyone"))
            {
                return ESRB.Everyone;
            }
            else if (text.Equals("Everyone"))
            {
                return ESRB.Everyone;
            }
            else if (text.Equals("Everyone 10+"))
            {
                return ESRB.Everyone;
            }
            else if (text.Equals("Teen"))
            {
                return ESRB.Teen;
            }
            else if (text.Equals("Mature"))
            {
                return ESRB.Mature;
            }
            else if (text.Equals("Adults Only"))
            {
                return ESRB.AO;
            }
            return ESRB.Null;
        }

        #endregion
    }
}
