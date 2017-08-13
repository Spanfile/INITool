using System.Collections.Generic;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Units
{
    internal class AssignmentUnit : Unit
    {
        public string Name => nameUnit.Name;
        public object Value => valueUnit.Value;

        private readonly NameUnit nameUnit;
        private readonly ValueUnit valueUnit;

        public AssignmentUnit(NameUnit nameUnit, ValueUnit valueUnit, IEnumerable<Token> sourceTokens) : base(sourceTokens)
        {
            this.nameUnit = nameUnit;
            this.valueUnit = valueUnit;
        }
    }
}
