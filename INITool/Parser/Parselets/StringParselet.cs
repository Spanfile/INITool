using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class StringParselet : Parselet, IPrefixParselet
    {
        public StringParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
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

            Token upcoming;
            while ((upcoming = Tokeniser.Peek()).TokenType != token.TokenType)
            {
                if (!isVerbatim)
                {
                    if (upcoming.TokenType == TokenType.Newline)
                        throw new InvalidTokenException(upcoming);
                }

                var next = Tokeniser.Take();
                tokens.Add(next);
                valueBuilder.Append(next.Value);
            }

            // consume the closing quote
            tokens.Add(Tokeniser.Take());
            return new ValueUnit(valueBuilder.ToString(), tokens.AsEnumerable());
        }
    }
}
