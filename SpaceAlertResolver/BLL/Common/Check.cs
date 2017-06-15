using System;

namespace BLL.Common
{
    /// <summary>
    /// Static routines for checking for exceptions on a single line.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Check for ArgumentNullException.
        /// Properly suppresses code analysis warning for checking if "argument" is not null before using.
        /// </summary>
        /// <param name="argument">Argument to check if it is not null.</param>
        /// <param name="name">Parameter name being checked.</param>
        public static void ArgumentIsNotNull([ValidatedNotNull]object argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
        }
    }
}
