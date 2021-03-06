﻿using System;
using System.Runtime.Serialization;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Parselets
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an error thrown when trying to parse an invalid value literal
    /// </summary>
    [Serializable]
    public class InvalidLiteralException : ParserException
    {
        internal InvalidLiteralException() : base("invalid literal")
        {
        }

        internal InvalidLiteralException(Position position) : base($"invalid literal at {position}")
        {
        }

        internal InvalidLiteralException(Exception inner) : base("invalid literal", inner)
        {
        }

        internal InvalidLiteralException(Position position, Exception inner) : base($"invalid literal at {position}", inner)
        {
        }

        protected InvalidLiteralException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
