using System;
using System.IO;
using System.Text;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class WordMatcher
    {
        public static Matcher.TokeniserDelegate Match(char character)
        {
            if (char.IsLetter(character))
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

                if (!char.IsLetter(character))
                    break;

                reader.Read();
                valueBuilder.Append(character);
            }

            return (TokenType.Word, valueBuilder.ToString());
        }
    }
}
