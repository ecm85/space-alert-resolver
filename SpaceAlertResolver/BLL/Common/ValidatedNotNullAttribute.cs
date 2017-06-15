using System;

namespace BLL.Common
{
    /// <summary>
    /// Static code analysis attribute for letting the code analyzer determine that a parameter will
    /// be validated to not be null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
