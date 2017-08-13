namespace INITool.Parser.Tokeniser
{
    internal enum TokenType
    {
        Empty, Unknown, Whitespace, Newline,
        Semicolon, Hash,
        LeftSquareBracket, RightSquareBracket,
        Equals, SingleQuote, DoubleQuote, Period, Dash, Ampersand, At,
        Letter, Digit
    }

    internal class Token
    {
        public static readonly Token Empty = new Token(TokenType.Empty, string.Empty);

        public TokenType TokenType { get; }
        public string Value { get; }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public override string ToString() => $"<{TokenType}: '{Value}'>";
    }
}
