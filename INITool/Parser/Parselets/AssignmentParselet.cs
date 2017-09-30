using System.Collections.Generic;
using System.Linq;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class AssignmentParselet : Parselet, IInfixParselet, ISingleUnitParselet
    {
        public AssignmentParselet(Parser parser, Tokeniser.Tokeniser tokeniser, IniOptions options)
            : base(parser, tokeniser, options)
        {
        }

        public Unit Parse(Unit left, Token token)
        {
            if (!(left is NameUnit))
                throw new InvalidUnitException(left, typeof(NameUnit));

            var name = (NameUnit)left;
            var value = Options.PropertyParsing == PropertyParsing.Strong ? ParseStrongValue() : ParseFlexibleValue();

            var tokens = name.SourceTokens.Append(token).Concat(value.SourceTokens);
            return new AssignmentUnit(name, value, tokens);
        }

        private ValueUnit ParseStrongValue() => Parser.ParseUnitOfType<ValueUnit>(skipNewline: false);

        private ValueUnit ParseFlexibleValue()
        {
            var first = Tokeniser.TakeAnyOtherThan(TokenType.Newline, TokenType.Empty);
            var tokens = new List<Token> {first};

            tokens.AddRange(Tokeniser.TakeSequentialOfAnyOtherThan(TokenType.Newline, TokenType.Empty));

            var value = string.Join("", tokens.Select(t => t.Value));

            return new ValueUnit(value, tokens.AsEnumerable());
        }

        public Unit TransformUnit(Unit left)
        {
            if (!Options.AllowLooseProperties)
                return left;

            if (!(left is NameUnit))
                throw new InvalidUnitException(left, typeof(NameUnit));

            var name = (NameUnit)left;
            return new AssignmentUnit(name, new ValueUnit(true, name.SourceTokens), name.SourceTokens);
        }
    }
}
