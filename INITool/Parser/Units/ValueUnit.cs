using System.Collections.Generic;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Units
{
    internal class ValueUnit : Unit
    {
        public object Value { get; }

        public ValueUnit(object value, IEnumerable<Token> sourceTokens) : base(sourceTokens)
        {
            Value = value;
        }
    }
}
