using System;
using System.Runtime.Serialization;

namespace INITool.Parser
{
    [Serializable]
    public class EndOfDocumentException : ParserException
    {
        internal EndOfDocumentException()
        {
        }

        internal EndOfDocumentException(string message) : base(message)
        {
        }

        internal EndOfDocumentException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EndOfDocumentException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}