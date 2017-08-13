using System;
using System.Runtime.Serialization;

namespace INITool.Parser.Tokeniser
{
    [Serializable]
    public class UnexpectedTokenException : ParserException
    {
        public UnexpectedTokenException(string message) : base(message)
        {
        }

        public UnexpectedTokenException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnexpectedTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}