using System;

namespace UniCade.Exceptions
{
    /// <summary>
    /// Custom UniCade exception template
    /// </summary>
    public class UniCadeException1 : Exception
    {
        public UniCadeException1()
        {
        }

        public UniCadeException1(string message)
            : base(message)
        {
        }

        public UniCadeException1(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
