using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class InvariantParselet : Parselet, IPrefixParselet
    {
        public InvariantParselet(Parser parser, Tokeniser.Tokeniser tokeniser, IniOptions options)
            : base(parser, tokeniser, options)
        {
        }

        public Unit Parse(Token token)
        {
            var tokens = new List<Token> {token};
            var valueBuilder = new StringBuilder(token.Value);

            foreach (var next in Tokeniser.TakeSequentialOfAnyOtherThan(TokenType.Empty, TokenType.Newline))
            {
                tokens.Add(next);
                valueBuilder.Append(next.Value);
            }

            return new ValueUnit(valueBuilder.ToString(), tokens.AsEnumerable());
        }
    }
}