using System.Linq;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class SectionParselet : Parselet, IPrefixParselet
    {
        public SectionParselet(Parser parser, Tokeniser.Tokeniser tokeniser, IniOptions options)
            : base(parser, tokeniser, options)
        {
        }

        public Unit Parse(Token token)
        {
            var name = Parser.ParseUnitOfType<NameUnit>(allowSingleUnitTransform: false);
            var rightSquare = Tokeniser.TakeOfType(TokenType.RightSquareBracket);
            return new SectionUnit(name, new [] {token}.Concat(name.SourceTokens).Append(rightSquare));
        }
    }
}
