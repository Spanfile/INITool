using System.Collections.Generic;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Units
{
    internal class NameUnit : Unit
    {
        public string Name { get; }

        public NameUnit(string name, IEnumerable<Token> sourceTokens) : base(sourceTokens)
        {
            Name = name;
        }
    }
}
