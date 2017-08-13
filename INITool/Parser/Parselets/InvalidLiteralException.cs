using System;
using System.Runtime.Serialization;

namespace INITool.Parser.Parselets
{
    [Serializable]
    public class InvalidLiteralException : ParserException
    {
        public InvalidLiteralException() : base("invalid literal")
        {
        }

        public InvalidLiteralException(Exception inner) : base($"invalid literal", inner)
        {
        }

        protected InvalidLiteralException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
