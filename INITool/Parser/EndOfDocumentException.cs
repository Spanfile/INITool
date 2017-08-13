using System;
using System.Runtime.Serialization;

namespace INITool.Parser
{
    [Serializable]
    public class EndOfDocumentException : ParserException
    {
        public EndOfDocumentException()
        {
        }

        public EndOfDocumentException(string message) : base(message)
        {
        }

        public EndOfDocumentException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EndOfDocumentException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}