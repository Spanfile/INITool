using System.Linq;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class AssignmentParselet : Parselet, IInfixParselet
    {
        public AssignmentParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
        }

        public Unit Parse(Unit left, Token token)
        {
            if (!(left is NameUnit))
                throw new UnexpectedUnitException(left, typeof(NameUnit));

            var name = (NameUnit)left;
            var value = Parser.ParseUnitOfType<ValueUnit>(false, true);
            var tokens = name.SourceTokens.Append(token).Concat(value.SourceTokens);
            return new AssignmentUnit(name, value, tokens);
        }
    }
}
