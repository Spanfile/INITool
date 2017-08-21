using System;
using System.Runtime.Serialization;

namespace INITool.Parser
{
    [Serializable]
    public class ParserException : Exception
    {
        internal ParserException()
        {
        }

        internal ParserException(string message) : base(message)
        {
        }

        internal ParserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
