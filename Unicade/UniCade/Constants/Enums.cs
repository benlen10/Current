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

        public enum ESRB
        {
            [StringValue("Everyone")]
            Everyone,
            [StringValue("Everyone 10+")]
            Everyone10
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

        #endregion
    }
}
