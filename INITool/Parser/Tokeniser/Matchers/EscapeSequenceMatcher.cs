using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class EscapeSequenceMatcher
    {
        public static Matcher.TokeniserDelegate Match(char character)
        {
            if (character == '\\')
                return Tokenise;

            return null;
        }

        public static (TokenType type, string value) Tokenise(char character, TextReader reader)
        {
            var valueBuilder = new StringBuilder(character.ToString());

            // TODO: unicode character sequences
            if (reader.Peek() != -1)
                valueBuilder.Append((char)reader.Read());

            return (TokenType.EscapeSequence, valueBuilder.ToString());
        }
    }
}
