using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class NumberMatcher
    {
        public static Matcher.TokeniserDelegate Match(char character)
        {
            if (char.IsDigit(character))
                return Tokenise;

            return null;
        }

        public static (TokenType type, string value) Tokenise(char character, TextReader reader)
        {
            var valueBuilder = new StringBuilder(character.ToString());

            while (true)
            {
                var peek = reader.Peek();
                if (peek == -1)
                    break;

                character = (char)peek;

                if (!char.IsDigit(character))
                    break;

                reader.Read();
                valueBuilder.Append(character);
            }

            return (TokenType.Number, valueBuilder.ToString());
        }
    }
}
