namespace INITool.Parser.Tokeniser
{
    internal enum TokenType
    {
        Empty, Unknown, Whitespace, Newline,
        Semicolon, Hash,
        LeftSquareBracket, RightSquareBracket,
        Equals, SingleQuote, DoubleQuote, Period, Dash, Ampersand, At,
        Word, Number, EscapeSequence
    }

    internal class Token
    {
        public static readonly Token Empty = new Token(TokenType.Empty, string.Empty, new Position(0, -1));

        public TokenType TokenType { get; }
        public string Value { get; }
        public Position Position { get; }

        public Token(TokenType tokenType, string value, Position position)
        {
            TokenType = tokenType;
            Value = value;
            Position = position;
        }

        public override string ToString() => $"<{Position}, {TokenType}: '{Value}'>";
    }
}
