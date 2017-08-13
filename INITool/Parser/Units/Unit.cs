using System.Collections.Generic;
using System.Linq;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Units
{
    internal abstract class Unit
    {
        public IEnumerable<Token> SourceTokens => sourceTokens;
        private readonly List<Token> sourceTokens;

        protected Unit(IEnumerable<Token> sourceTokens)
        {
            this.sourceTokens = new List<Token>(sourceTokens);
        }

        public string SourceTokenString() => string.Join("", sourceTokens.Select(t => t.Value));
    }
}
