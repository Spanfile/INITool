using System.Linq;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class SectionParselet : Parselet, IPrefixParselet
    {
        public SectionParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
        }

        public Unit Parse(Token token)
        {
            var name = Parser.ParseUnitOfType<NameUnit>();
            var rightSquare = Tokeniser.TakeOfType(TokenType.RightSquareBracket);
            return new SectionUnit(name, new [] {token}.Concat(name.SourceTokens).Append(rightSquare));
        }
    }
}
