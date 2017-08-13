using System.Collections.Generic;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Units
{
    internal class SectionUnit : Unit
    {
        public string Name => nameUnit.Name;

        private readonly NameUnit nameUnit;

        public SectionUnit(NameUnit nameUnit, IEnumerable<Token> sourceTokens) : base(sourceTokens)
        {
            this.nameUnit = nameUnit;
        }
    }
}
