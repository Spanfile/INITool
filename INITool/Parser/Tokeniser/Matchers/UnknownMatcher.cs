using System;
using System.IO;
using System.Text;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class UnknownMatcher
    {
        public static Matcher.TokeniserDelegate Match(char character)
        {
            // unknown matcher always matches
            return Tokenise;
        }

        // unknown tokens are always singular characters
        public static (TokenType type, string value) Tokenise(char character, TextReader reader) => (TokenType.Unknown, character.ToString());
    }
}
