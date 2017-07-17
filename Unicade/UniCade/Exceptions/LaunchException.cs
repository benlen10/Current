using System;

namespace UniCade.Exceptions
{
    /// <summary>
    /// Custom UniCade exception template
    /// </summary>
    public class LaunchException : Exception
    {
        public LaunchException()
        {
        }

        public LaunchException(string message)
            : base(message)
        {
        }

        public LaunchException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
