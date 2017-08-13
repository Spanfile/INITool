using System;
using System.Runtime.Serialization;
using INITool.Parser.Tokeniser;

namespace INITool.Parser
{
    [Serializable]
    internal class InvalidTokenException : ParserException
    {
        public InvalidTokenException(Token invalidToken) : base($"invalid token: {invalidToken}")
        {
        }

        public InvalidTokenException(Token invalidToken, Exception inner) : base($"invalid token: {invalidToken}", inner)
        {
        }

        protected InvalidTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
