using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class StringParselet : Parselet, IPrefixParselet
    {
        private readonly Dictionary<char, char> escapeSequences;

        public StringParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
            escapeSequences = new Dictionary<char, char> {
                 {'b', '\b'},
                 {'n', '\n'},
                 {'r', '\r'},
                 {'t', '\t'},
                 {'v', '\v' },
                 {'\\', '\\'},
                 {'\'', '\''},
                 {'"', '\"'}
            };
        }

        public Unit Parse(Token token)
        {
            var tokens = new List<Token> { token };
            var valueBuilder = new StringBuilder();
            var isVerbatim = false;

            if (token.TokenType == TokenType.At)
            {
                var next = Tokeniser.Peek();
                if (next.TokenType != TokenType.SingleQuote && next.TokenType != TokenType.DoubleQuote)
                    throw new InvalidTokenException(token);

                isVerbatim = true;
                Tokeniser.Take();
                tokens.Add(next);
                token = next;
            }

            while (Tokeniser.Peek().TokenType != token.TokenType)
            {
                var next = Tokeniser.Take();
                if (!isVerbatim)
                {
                    if (next.TokenType == TokenType.Newline)
                        throw new InvalidTokenException(next);

                    if (next.TokenType == TokenType.EscapeSequence)
                    {
                        var escChar = next.Value.Substring(1)[0];

                        if (!escapeSequences.TryGetValue(escChar, out var escaped))
                            throw new InvalidTokenException(next);

                        tokens.Add(next);
                        valueBuilder.Append(escaped);
                        continue;
                    }
                }

                tokens.Add(next);
                valueBuilder.Append(next.Value);
            }

            // consume the closing quote
            tokens.Add(Tokeniser.Take());

            return new ValueUnit(valueBuilder.ToString(), tokens.AsEnumerable());
        }
    }
}
