using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class NameParselet : Parselet, IPrefixParselet
    {
        public NameParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
        }

        public Unit Parse(Token token)
        {
            var tokens = new List<Token> {token};
            var nameBuilder = new StringBuilder(token.Value);

            foreach (var next in Tokeniser.TakeSequentialOfType(TokenType.Letter, TokenType.Digit))
            {
                tokens.Add(next);
                nameBuilder.Append(next.Value);
            }

            var name = nameBuilder.ToString();

            if (name.ToLower() == "true" || name.ToLower() == "false")
                return new ValueUnit(name.ToLower() == "true", tokens.AsEnumerable());

            return new NameUnit(nameBuilder.ToString(), tokens.AsEnumerable());
        }
    }
}
