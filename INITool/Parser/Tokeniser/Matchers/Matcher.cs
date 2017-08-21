using System.IO;

namespace INITool.Parser.Tokeniser.Matchers
{
    internal static class Matcher
    {
        public delegate (TokenType type, string value) TokeniserDelegate(char character, TextReader reader);

        public delegate TokeniserDelegate MatcherDelegate(char character);
    }
}
