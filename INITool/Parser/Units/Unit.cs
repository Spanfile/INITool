using System.Collections.Generic;
using System.Linq;
using INITool.Parser.Tokeniser;

namespace INITool.Parser.Units
{
    internal abstract class Unit
    {
        public string Comment { get; set; }

        public Position Position => SourceTokens.First().Position;

        public IEnumerable<Token> SourceTokens { get; }

        protected Unit(IEnumerable<Token> sourceTokens)
        {
            SourceTokens = new List<Token>(sourceTokens);
            Comment = "";
        }

        public string SourceTokenString() => string.Join("", SourceTokens.Select(t => t.Value));
    }
}
