using System.Collections.Generic;
using System.IO;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class SingleCharacterMatcher
    {
        private static readonly Dictionary<char, TokenType> CharDict = new Dictionary<char, TokenType> {
            {';', TokenType.Semicolon},
            {'#', TokenType.Hash},
            {'[', TokenType.LeftSquareBracket},
            {']', TokenType.RightSquareBracket},
            {'=', TokenType.Equals},
            {'\'', TokenType.SingleQuote},
            {'"', TokenType.DoubleQuote},
            {'.', TokenType.Period},
            {'-', TokenType.Dash},
            {'&', TokenType.Ampersand},
            {'@', TokenType.At}
        };

        public static Matcher.TokeniserDelegate Match(char character)
        {
            if (CharDict.ContainsKey(character))
                return Tokenise;

            return null;
        }

        public static (TokenType type, string value) Tokenise(char character, TextReader reader) => (CharDict[character], character.ToString());
    }
}
