using System;
using System.IO;
using System.Text;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class NewlineMatcher
    {
        public static Matcher.TokeniserDelegate Match(char character)
        {
            if (character == '\n' || character == '\r')
                return Tokenise;

            return null;
        }

        public static (TokenType type, string value) Tokenise(char character, TextReader reader)
        {
            var valueBuilder = new StringBuilder(character.ToString());

            // ReSharper disable once InvertIf
            if (character == '\r')
            {
                if (reader.Peek() != '\n')
                    throw new ArgumentException("incomplete newline"); // TODO: better exception

                valueBuilder.Append((char)reader.Read());
            }

            return (TokenType.Newline, valueBuilder.ToString());
        }
    }
}
