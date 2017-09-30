using System;
using System.Runtime.Serialization;

namespace INITool
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an error thrown when trying to parse an invalid property identifier
    /// </summary>
    [Serializable]
    public class InvalidPropertyIdentifierException : Exception
    {
        internal InvalidPropertyIdentifierException(string identifier) : base(
            $"invalid property identifier: {identifier}")
        {
        }

        internal InvalidPropertyIdentifierException(string identifier, Exception inner) : base(
            $"invalid property identifier: {identifier}", inner)
        {
        }

        protected InvalidPropertyIdentifierException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}