using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal interface IPrefixParselet
    {
        Unit Parse(Token token);
    }
}
