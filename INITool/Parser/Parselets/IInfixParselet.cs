using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal interface IInfixParselet
    {
        Unit Parse(Unit left, Token token);
    }
}
