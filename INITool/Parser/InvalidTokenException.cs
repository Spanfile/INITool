using System;
using System.Runtime.Serialization;
using INITool.Parser.Tokeniser;

namespace INITool.Parser
{
    [Serializable]
    public class InvalidTokenException : ParserException
    {
        internal InvalidTokenException(Token invalidToken) : base($"invalid token: {invalidToken}")
        {
        }

        internal InvalidTokenException(Token invalidToken, string append) : base($"invalid token: {invalidToken}. {append}")
        {
        }

        internal InvalidTokenException(Token invalidToken, Exception inner) : base($"invalid token: {invalidToken}", inner)
        {
        }

        internal InvalidTokenException(Token invalidToken, string append, Exception inner) : base($"invalid token: {invalidToken}. {append}", inner)
        {
        }

        protected InvalidTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
