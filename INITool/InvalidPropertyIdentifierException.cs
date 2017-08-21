using System;
using System.Runtime.Serialization;

namespace INITool
{
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